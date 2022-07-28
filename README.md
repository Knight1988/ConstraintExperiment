# ConstraintExperiment

Test performance between database with constraint and non constraint
Tests are perform on few server

|Server|Report|
|--|--|
|MS SQL Server|[TestReport](TestReport-Mssql.md)|
|Postgres|[TestReport](TestReport-Postgres.md)|



## Conclution

There is almost no different when select, insert and update.
But it has huge impact on delete because DB has to delete related record on another tables
