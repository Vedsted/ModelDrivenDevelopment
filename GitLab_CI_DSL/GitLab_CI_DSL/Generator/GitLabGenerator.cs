using System;
using System.Collections.Generic;
using System.Linq;
using GitLab_CI_DSL.metamodel.@default;
using GitLab_CI_DSL.metamodel.jobs;
using GitLab_CI_DSL.metamodel.pipeline;
using GitLab_CI_DSL.metamodel.stage;

namespace GitLab_CI_DSL.Generator
{
    public class GitLabYmlGenerator
    {
        public void CreateGitlabCiConfig(string fileName, string filePath, Pipeline pipeline)
        {
            var s = parseDefaultJob(pipeline.Default);
            
            s += ParseStages(pipeline.Stages);
            
            pipeline.Stages.ForEach(stage => s += ParseJobs(stage));
            
            Console.Write(s);
        }

        private string ParseStages(List<Stage> stages)
        {
            var stageNames = stages.Select(stage => stage.Name).ToList();
            var s = $"stages:\n";
            stageNames.ForEach(stage => s+= $"  - {stage}\n");

            return s;
        }


        private string parseDefaultJob(Default job)
        {
            if (job == null)
                return "";
            

            var s = "default:\n";
            
            
            if (job.Image != null)
                s += $"  image: {job.Image}\n";


            var variables = job.EnvironmentVariables;
            if (variables != null)
            {
                s += $"  variables:\n";
                foreach (var kv in variables)
                {
                    s += $"    {kv.Key}: \"{kv.Value}\"\n";
                }
            }

            return s;
        }

        public string ParseJobs(Stage stage)
        {
            var s = "";
            stage.Jobs.FindAll(job => job.Type == JobType.JOB ).ForEach( job => s += ParseJob(stage.Name, job) );

            return s;
        }

        private string ParseJob(string stageName, Job job)
        {
            List<Job> extends = new List<Job>();
            GetJobParents(extends, job);

            string image = null; 
            Dictionary<string, string> variables = new Dictionary<string, string>();
            List<string> scripts = new List<string>();
            
            extends.ForEach(j =>
            {
                if (j.Image != null)
                    image = j.Image;

                if (j.Variables != null)
                {
                    foreach (var (key, value) in j.Variables)
                    {
                        variables.Add(key, value);
                    }
                }
                
                if(j.Scripts != null)
                    j.Scripts.ForEach(script => scripts.Add(script));
            });
            
            var s = $"{job.Name}:\n";
            s += $"  stage: {stageName}\n";
            
            if (image != null)
                s += $"  image: {image}\n";
            

            if (variables.Count != 0)
            {
                s += $"  variables:\n";
                foreach (var kv in variables)
                {
                    s += $"    {kv.Key}: \"{kv.Value}\"\n";
                }
            }
                
            if (scripts.Count != 0)
            {
                s += $"  script:\n";
                scripts.ForEach(script => s+= $"    - {script}\n");
            }

            return s;
        }

        private void GetJobParents(List<Job> jobs, Job job)
        {
            if (job.Extends != null)
            {
                job.Extends.ForEach( j => GetJobParents(jobs, j));
            }
            
            jobs.Add(job);
        }
        
    }
    
    
    
}