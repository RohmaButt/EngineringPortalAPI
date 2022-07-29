using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.DTOs.KPIDashboard
{
    public class KPIComboDataDto
    {
        #region Ctor
        public KPIComboDataDto()
        { }

        public KPIComboDataDto(string value, bool isMega)
        {
            this.Value = value;
            this.IsMega = isMega;
        }

        public KPIComboDataDto(string value)
        {
            this.Value = value;
            this.IsMega = false;
        }
        #endregion

        public string Value { get; set; }

        public bool IsMega { get; set; }
    }
}