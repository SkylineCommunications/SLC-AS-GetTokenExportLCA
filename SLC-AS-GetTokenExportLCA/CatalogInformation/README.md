# SLC-AS-GetTokenExportLCA

This Automation Script retrieves a token that allows exporting a specific **Low-Code App (LCA)** or **Dashboard** from the system where the script is executed.

Once the token is generated, it can be used with the following URL:  
*https://[dmaip]/API/v1/GetSecureFile.aspx?token=[token]*

The download is a `.zip` file identical to the one produced by DIS (*Add > Import Low-Code App* / *Add > Import Dashboard*) and can be placed directly in the `PackageContent\LowCodeApps` or `PackageContent\Dashboards` folder of a DIS package project.

## Parameters

| Parameter | Type = `LCA` | Type = `Dashboard` |
|---|---|---|
| **LCA ID / Dashboard Name** | ID (GUID) of the Low-Code App | Name of the dashboard as shown in the Dashboards app |
| **LCA Version / Dashboard Folder** | Version number (`0` = latest published version) | Folder path of the dashboard (use `/` for the root folder) |
| **Type** | `LCA` (default) | `Dashboard` |

## Notes

- If the **version** parameter is set to `0`, the script retrieves the **latest published version** of the LCA.
- For dashboards in the root folder, enter `/` as folder (the script sends an empty folder to the API).
- LCAs are exported through `internal.asmx/ExportApplication`; dashboards through `dashboards.asmx/ExportDashboards`.
- The token is valid only for the current system and security context. Download the file directly from the DataMiner Agent (not through a cloud gateway URL).
