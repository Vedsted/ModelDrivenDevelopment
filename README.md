# Model-Driven Software Development
Repository for MDSD course on SDU.

## Future ideas for internal pipeline DSL
- Cahce bewteen jobs
    ```
    Stage("save_artifacts").
        Job("javadoc").
            Cache("target/*").
    ```
- Artifacts from pipeline
    ```
    Stage("save_artifacts").
        Job("javadoc").
            Artifact("target/apidocs").
                Expire(1, WEEK).
    ```

- Rules for which branches the job apply
    ```
    Stage("deployment").
        Job("deploy").
            OnBranch("master").
    ```
