using System;
using System.Collections.Generic;

namespace GitLab_CI_DSL
{
    public class Default
    {
        public string Image { get; private set; }
        public Dictionary<string, string> EnvironmentVariables { get; private set; }

        public void AddEnvironmentVariable(string key, string value)
        {
            if (EnvironmentVariables == null)
            {
                EnvironmentVariables = new Dictionary<string, string>();
            }
            this.EnvironmentVariables.Add(key, value);
        }

        public void SetImage(string imageName)
        {
            if (Image != null)
            {
                throw new Exception($"Error trying to set multiple images in Default! Error occured for image: '{imageName}').");
            }

            Image = imageName;
        }
        
    }
}