using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.Models
{
    public class CheckBoxListInfo
    {
        public string Value { get; private set; }
        public string DisplayText { get; private set; }
        public bool IsChecked { get; private set; }

        public CheckBoxListInfo(string value, string displayText, bool isChecked)
        {
            this.Value = value;
            this.DisplayText = displayText;
            this.IsChecked = IsChecked;
        }
    }
}