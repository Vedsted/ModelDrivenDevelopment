using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitLab_CI_DSL
{
    public class YmlCreator
    {
        public void CreateGitlabCiConfig(Pipeline pipeline)
        {
            var s = parseDefaultJob(pipeline.Default);

            var stages = pipeline.Stages.Select(stage => stage.Name).ToList();
            s += $"stages:\n";
            stages.ForEach(stage => s+= $"  - {stage}\n");

            s += parseJobs(pipeline.Extensions);
            s += parseJobs(pipeline.Jobs);
            
            Console.Write(s);
        }


        private string parseDefaultJob(DefaultJob job)
        {
            if (job == null)
            {
                return "";
            }

            var s = "default:\n";
            
            var image = job.Image;
            if (image != null)
            {
                s += $"  image: {image}\n";
            }


            var envars = job.EnvironmentVariables;
            if (envars.Count != 0)
            {
                s += "  variables:\n";
                envars.ForEach(pair => s+= $"    {pair.Key}: \"{pair.Value}\"\n");
            }

            job.Scripts.ForEach(script => s+= $"  script: {script}");
            
            return s;
        }

        private string parseJobs(List<Job> extensions)
        {
            if (extensions.Count == 0)
            {
                return "";
            }

            var s = "";
            
            extensions.ForEach(job => s += parseJob(job));

            return s;
        }

        private string parseJob(Job job)
        {
            var s = $"{job.Name}:\n";

            if (job.Stage != null)
            {
                s += $"  stage: {job.Stage}\n";
            }

            if (job.Extension != null)
            {
                s += $"  extends: {job.Extension}\n";
            }

            if (job.Image != null)
            {
                s += $"  image: {job.Image}\n";
            }

            if (job.EnvironmentVariables.Count != 0)
            {
                s += $"  variables:\n";
                job.EnvironmentVariables.ForEach(pair => s+= $"    {pair.Key}: \"{pair.Value}\"\n");
            }
                
            if (job.Scripts.Count != 0)
            {
                s += $"  script:\n";
                job.Scripts.ForEach(script => s+= $"    - {script}\n");
            }

            return s;
        }
    }
    
    
    
}