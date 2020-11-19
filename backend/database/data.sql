-- *****************************************************************************
-- This script contains INSERT statements for populating tables with seed data
-- *****************************************************************************

BEGIN TRANSACTION;

-- default username of 'user' and default password of 'greatwall'
INSERT INTO users
  (username,password,salt,role)
VALUES
  ('user', 'jUE98uhvS5tdIlxRsmz1W7/Qaqo=', '9CWPUTvXqQ4=', 'User')

insert into PotHole(PotHoleLocation, DateReported, Severity, RepairStatus, RepairBeginDate, RepairFinishDate)
values
('3798 West 39th St', '2020-04-06', 5, 'Not Repaired', null, null),
('7100 Euclid Avenue', '2020-01-13', 2, 'Repaired', '2020-01-01', '2020-01-03'),
('3900 Wildlife Way', '2019-08-13', 1, 'Not Repaired', null, null)

  COMMIT TRANSACTION;
