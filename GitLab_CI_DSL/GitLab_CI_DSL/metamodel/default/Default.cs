using System;
using System.Collections.Generic;

namespace GitLab_CI_DSL
{
    public class Default
    {
        public string Image { get; set; }
        public Dictionary<string, string> EnvironmentVariables { get; private set; } // Consider dicts

        public void AddEnvironmentVariable(string key, string value)
        {
            if (EnvironmentVariables == null)
            {
                EnvironmentVariables = new Dictionary<string, string>();
            }
            this.EnvironmentVariables.Add(key, value);
        }
        
    }
}