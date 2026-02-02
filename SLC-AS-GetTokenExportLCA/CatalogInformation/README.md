# SLC-AS-GetTokenExportLCA

This Automation Script retrieves a token that allows exporting a specific **LCA** from the system where the script is executed.

Once the token is generated, it can be used with the following URL:  
*https://[dmaip]/API/v1/GetSecureFile.aspx?token=[token]*

## Notes

- If the **version** parameter is set to `0`, the script retrieves the **latest published version** of the LCA.
- The token is valid only for the current system and security context.