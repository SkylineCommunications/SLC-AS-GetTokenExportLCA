# SLC-AS-GetTokenExportLCA

This Automation Script retrieves a token that allows exporting a specific **Low-Code App (LCA)** or **Dashboard** from the system where the script is executed.

Once the token is generated, it can be used with the following URL:  
*https://[dmaip]/API/v1/GetSecureFile.aspx?token=[token]*

The download is a `.zip` file identical to the one produced by DIS (*Add > Import Low-Code App* / *Add > Import Dashboard*) and can be placed directly in the `PackageContent\LowCodeApps` or `PackageContent\Dashboards` folder of a DIS package project. This is useful for agents that are not reachable by DIS (e.g. only accessible through a cloud gateway), where the DIS import fails on the `GetSecureFile.aspx` download step.

## Parameters

| Parameter | Type = `LCA` | Type = `Dashboard` |
|---|---|---|
| **LCA ID / Dashboard Name** | ID (GUID) of the Low-Code App | Name of the dashboard as shown in the Dashboards app |
| **LCA Version / Dashboard Folder** | Version number (`0` = latest published version) | Folder path of the dashboard (use `/` for the root folder) |
| **Type** | `LCA` (default) | `Dashboard` |

## How it works

1. Requests an authentication ticket over the user's SLNet connection and calls `internal.asmx/ConnectWithTicket` to obtain a Web API connection ID.
2. Depending on **Type**:
   - `LCA`: calls `internal.asmx/ExportApplication` with the application ID and version.
   - `Dashboard`: calls `dashboards.asmx/ExportDashboards` with `{ Name, Folder }`.
3. Logs the returned secure-file token in the Information Events.
4. Download the file with `https://[dmaip]/API/v1/GetSecureFile.aspx?token=[token]`.

## Notes

- If the **version** parameter is set to `0`, the script retrieves the **latest published version** of the LCA.
- For dashboards in the root folder, enter `/` as folder (the script sends an empty folder to the API).
- The token is valid only for the current system and security context. Download the file directly from the DataMiner Agent (not through a cloud gateway URL).
