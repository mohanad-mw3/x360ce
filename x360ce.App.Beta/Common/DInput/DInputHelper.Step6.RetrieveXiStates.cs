﻿using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x360ce.App.DInput
{
	public partial class DInputHelper
	{

		public Controller[] XiControllers;
		public State[] LiveXInputStates;
		public bool[] XiControllerConnected;

		void RetrieveXiStates()
		{
			// Allow if not testing or testing with option enabled.
			var o = SettingsManager.Options;
			var allow = !o.TestEnabled || o.TestGetXInputStates;
			lock (Controller.XInputLock)
			{
				// Before states can be retrieved xinput configuration must be checked.
				for (uint i = 0; i < 4; i++)
				{
					var gamePad = XiControllers[i];
					if (Controller.IsLoaded && gamePad.IsConnected && allow)
					{
						LiveXInputStates[i] = gamePad.GetState();
						XiControllerConnected[i] = true;
					}
					else
					{
						LiveXInputStates[i] = new State();
						XiControllerConnected[i] = false;
					}
				}
			}
			var ev = StatesRetrieved;
			if (ev != null)
				ev(this, new EventArgs());
		}

	}
}
