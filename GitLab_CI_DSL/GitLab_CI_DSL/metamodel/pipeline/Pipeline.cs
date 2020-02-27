using System;
using System.Collections.Generic;
using GitLab_CI_DSL.metamodel.@default;
using GitLab_CI_DSL.metamodel.jobs;
using GitLab_CI_DSL.metamodel.stage;

namespace GitLab_CI_DSL.metamodel.pipeline
{
    public class Pipeline
    {
        public Default Default { get; private set; }

        public List<Stage> Stages { get; private set; }

        public List<Job> AbstractJobs { get; private set; }

        public void AddStage(Stage stage)
        {
            if(Stages == null)
                Stages = new List<Stage>();
            
            Stages.Add(stage);
        }

        public void AddAbstractJob(Job abstractJob)
        {
            if (AbstractJobs == null)
                AbstractJobs = new List<Job>();

            if (abstractJob.Type != JobType.ABSTRACTJOB)
                throw new Exception($"Error adding abstract job to pipeline! Job with name: '{abstractJob.Name}' is not of type AbstractJob");
            
            AbstractJobs.Add(abstractJob);
        }


        public void SetDefault(Default def)
        {
            if (Default != null)
                throw new Exception("Only one 'Default' can be set for the pipeline");

            Default = def;
        }

        public void Validate()
        {
            Console.WriteLine("Validate.... \n");
        }
    }
}