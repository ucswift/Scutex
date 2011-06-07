using System;
using System.Management;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Providers.WmiDataProvider.Properties;

namespace WaveTech.Scutex.Providers.WmiDataProvider
{
	internal class WmiDataProvider : IWmiDataProvider
	{
		public string GetProcessorData()
		{
			return GetCpuId() + GetCpuManufacturer() + GetCpuSocket() + GetCpuVersion();
		}

		public string GetMotherboardData()
		{
			return GetMotherboardManufacturer() + GetMotherboardSku() + GetMotherboardVersion();
		}

		public string GetBiosData()
		{
			return GetBiosName() + GetBiosVersion();
		}


		public string GetCpuManufacturer()
		{
			string cpuMan = String.Empty;
			ManagementClass mgmt = new ManagementClass(Resources.Wmi_Processsor);

			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				if (cpuMan == String.Empty)
				{
					if (obj.Properties[Resources.Wmi_Processsor_Manufacturer].Value != null)
						cpuMan = obj.Properties[Resources.Wmi_Processsor_Manufacturer].Value.ToString();
				}
			}
			return cpuMan;
		}

		public string GetCpuSocket()
		{
			string cpuMan = String.Empty;
			ManagementClass mgmt = new ManagementClass(Resources.Wmi_Processsor);

			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				if (cpuMan == String.Empty)
				{
					if (obj.Properties[Resources.Wmi_Processsor_SocketDesignation].Value != null)
						cpuMan = obj.Properties[Resources.Wmi_Processsor_SocketDesignation].Value.ToString();
				}
			}
			return cpuMan;
		}

		public string GetCpuId()
		{
			string cpuMan = String.Empty;
			ManagementClass mgmt = new ManagementClass(Resources.Wmi_Processsor);

			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				if (cpuMan == String.Empty)
				{
					if (obj.Properties[Resources.Wmi_Processsor_ProcessorId].Value != null)
						cpuMan = obj.Properties[Resources.Wmi_Processsor_ProcessorId].Value.ToString();
				}
			}
			return cpuMan;
		}

		public string GetCpuVersion()
		{
			string cpuMan = String.Empty;
			ManagementClass mgmt = new ManagementClass(Resources.Wmi_Processsor);

			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				if (cpuMan == String.Empty)
				{
					if (obj.Properties[Resources.Wmi_Processsor_Version].Value != null)
						cpuMan = obj.Properties[Resources.Wmi_Processsor_Version].Value.ToString();
				}
			}
			return cpuMan;
		}


		public string GetMotherboardManufacturer()
		{
			string cpuMan = String.Empty;
			ManagementClass mgmt = new ManagementClass(Resources.Wmi_Motherboard);

			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				if (cpuMan == String.Empty)
				{
					if (obj.Properties[Resources.Wmi_Motherboard_Manufacturer].Value != null)
						cpuMan = obj.Properties[Resources.Wmi_Motherboard_Manufacturer].Value.ToString();
				}
			}
			return cpuMan;
		}

		public string GetMotherboardVersion()
		{
			string cpuMan = String.Empty;
			ManagementClass mgmt = new ManagementClass(Resources.Wmi_Motherboard);

			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				if (cpuMan == String.Empty)
				{
					if (obj.Properties[Resources.Wmi_Motherboard_Version].Value != null)
						cpuMan = obj.Properties[Resources.Wmi_Motherboard_Version].Value.ToString();
				}
			}
			return cpuMan;
		}

		public string GetMotherboardSku()
		{
			string cpuMan = String.Empty;
			ManagementClass mgmt = new ManagementClass(Resources.Wmi_Motherboard);

			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				if (cpuMan == String.Empty)
				{
					if (obj.Properties[Resources.Wmi_Motherboard_Sku].Value != null)
						cpuMan = obj.Properties[Resources.Wmi_Motherboard_Sku].Value.ToString();
				}
			}
			return cpuMan;
		}

		public string GetMotherboardTag()
		{
			string cpuMan = String.Empty;
			ManagementClass mgmt = new ManagementClass(Resources.Wmi_Motherboard);

			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				if (cpuMan == String.Empty)
				{
					if (obj.Properties[Resources.Wmi_Motherboard_Tag].Value != null)
						cpuMan = obj.Properties[Resources.Wmi_Motherboard_Tag].Value.ToString();
				}
			}
			return cpuMan;
		}


		public string GetBiosName()
		{
			string cpuMan = String.Empty;
			ManagementClass mgmt = new ManagementClass(Resources.Wmi_Bios);

			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				if (cpuMan == String.Empty)
				{
					if (obj.Properties[Resources.Wmi_Bios_Name].Value != null)
						cpuMan = obj.Properties[Resources.Wmi_Bios_Name].Value.ToString();
				}
			}
			return cpuMan;
		}

		public string GetBiosVersion()
		{
			string cpuMan = String.Empty;
			ManagementClass mgmt = new ManagementClass(Resources.Wmi_Bios);

			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				if (cpuMan == String.Empty)
				{
					if (obj.Properties[Resources.Wmi_Bios_Version].Value != null)
						cpuMan = obj.Properties[Resources.Wmi_Bios_Version].Value.ToString();
				}
			}
			return cpuMan;
		}
	}
}
