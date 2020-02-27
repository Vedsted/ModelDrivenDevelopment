using System;
using System.Collections.Generic;
using GitLab_CI_DSL.metamodel.jobs;

namespace GitLab_CI_DSL.metamodel.stage
{
    public class Stage
    {
        public string Name { get; }
        public List<Job> Jobs { get; private set; }

        public Stage(string name)
        {
            Name = name;
        }

        public void AddJob(Job job)
        {
            if (Jobs == null)
            {
                Jobs = new List<Job>();
            }

            if (Jobs.Find( j => j.Name == job.Name) != null)
            {
                throw new Exception($"Multiple jobs with the same name is not allowed! Rename job with name: '{job.Name}'");
            }
            
            Jobs.Add(job);
            
        }
        
        
    }
}