CREATE TABLE Characters (
    Id INTEGER PRIMARY KEY,
    Name TEXT,
    CorpName TEXT,
    CloneSP INTEGER,
    SP INTEGER,
    SecStatus REAL,
    CloneName TEXT,
    SkillQueue TEXT,
    ApiKeyId INTEGER
);

CREATE TABLE ApiKeys (
    Id INTEGER PRIMARY KEY,
    Secret TEXT
);