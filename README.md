
evefifo
=======

A character+market tracker for EVE Online players, based on FIFO accounting

Notes
=====

Database
--------
Use `sqlite3 -init model\scripts\create-sqlite-db.sql c:\temp\evefifo.sqlite` to initialise the database for sqlite

TODO: set up option for SQL Server (just a different connection string?)

Credits
=======

- [ezet/evelib](https://github.com/ezet/evelib) used for accessing EVE APIs, distributed under Apache license v2