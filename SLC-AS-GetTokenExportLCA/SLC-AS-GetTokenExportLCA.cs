/*
****************************************************************************
*  Copyright (c) 2026,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

27/01/2026	1.0.0.1		RDU, Skyline	Initial version
****************************************************************************
*/

namespace GetTokenExportLCA
{
	using System;
	using System.Threading.Tasks;

	using GetTokenExportLCA.Dataclasses;

	using Skyline.DataMiner.Automation;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		private WebAPI _webAPI;
		private string _lcaID;
		private int _lcaVersion;

		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			try
			{
				var connection = engine.GetUserConnection();
				_webAPI = new WebAPI(connection, engine);

				if (!TryImportLCAID(engine) || !TryImportLCAVersion(engine))
				{
					engine.GenerateInformation("Invalid input parameters.");
					return;
				}

				var connectionId = GetFromWebAPIConnectionID();

				var token = GetTokenToExportLCA(connectionId, _lcaID, _lcaVersion);
				engine.GenerateInformation($"Token to export LCA: {token}");
			}
			catch (Exception e)
			{
				engine.Log($"Exception thrown:{Environment.NewLine}{e}");
			}
		}

		private string GetFromWebAPIConnectionID()
		{
			return GetFromWebAPIConnectionIDAsync().GetAwaiter().GetResult();
		}

		private async Task<string> GetFromWebAPIConnectionIDAsync()
		{
			return await _webAPI.GetWebAPIConnectionId();
		}

		private string GetTokenToExportLCA(string connectionId, string lcaId, int lcaVersion)
		{
			return _webAPI.GetTokenToExportLCA(connectionId, lcaId, lcaVersion).GetAwaiter().GetResult();
		}

		private bool TryImportLCAID(IEngine engine)
		{
			string lcaID = engine.GetScriptParam(10).Value;
			if (string.IsNullOrWhiteSpace(lcaID))
			{
				return false;
			}

			_lcaID = lcaID;
			return true;
		}

		private bool TryImportLCAVersion(IEngine engine)
		{
			if (!int.TryParse(engine.GetScriptParam(11).Value, out _lcaVersion))
			{
				return false;
			}

			return true;
		}
	}
}