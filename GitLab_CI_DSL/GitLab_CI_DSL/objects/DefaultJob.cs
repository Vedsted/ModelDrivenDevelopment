using System;
using System.Collections.Generic;

namespace GitLab_CI_DSL
{
    public class DefaultJob
    {
        public string Image { get; set; }
        public List<KeyValuePair<string, string>> EnvironmentVariables { get; }
        public List<string> Scripts { get; }
        
        public DefaultJob()
        {
            Scripts = new List<string>();
            EnvironmentVariables = new List<KeyValuePair<string, string>>();
        }

        public void AddEnvironmentVariable(string key, string value)
        {
            this.EnvironmentVariables.Add(new KeyValuePair<string, string>(key, value));
        }
        
        public void AddScript(string script)
        {
            Scripts.Add(script);
        }
    }
}