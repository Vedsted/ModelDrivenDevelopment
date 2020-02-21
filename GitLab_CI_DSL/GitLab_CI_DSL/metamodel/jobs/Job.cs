using System;
using System.Collections.Generic;

namespace GitLab_CI_DSL
{
    public class Job
    {
        public string Name { get; }

        public JobType Type { get; }
        
        public string Stage { get; set; }
        
        public List<string> Scripts { get; private set; }

        public List<Job> Extends { get; private set; }

        public Dictionary<string, string> Variables { get; private set; }


        public Job(string name, JobType type)
        {
            if (!name.StartsWith("."))
            {
                throw new ArgumentException("Extension name must start with '.'!");
            }
            Name = name;
            Type = type;
        }
        
        public void AddScript(string script)
        {
            if (Scripts == null)
            {
                Scripts = new List<string>();
            }
            Scripts.Add(script);
        }

        public void AddExtension(Job job)
        {
            if (Extends == null)
            {
                Extends = new List<Job>();
            }

            if (job.Type != JobType.ABSTRACTJOB)
            {
                throw new Exception($"Extending Job '{job.Name}' of type '{job.Type}' is not allowed! Jobs can only extend Jobs of type 'ABSTRACTJOB'");
            }

            Extends.Add(job);
        }

        public void AddEnvironmentVariable(string key, string value)
        {
            if (Variables == null)
            {
                Variables = new Dictionary<string, string>();
            }
            
            Variables.Add(key, value);
        }
    }
}