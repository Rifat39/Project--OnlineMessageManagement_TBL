create database OnlineMessageManagement;
use OnlineMessageManagement;

create table SocialService(
ServiceId int identity,
ServiceName varchar(50) not null,
ServiceStatus int not null default 1,
  CONSTRAINT ck_status CHECK (ServiceStatus IN (1,0))
);
----StoredProcedure-----


CREATE PROCEDURE GetAllSocialServices
AS
BEGIN
    SELECT ServiceId, ServiceName,ServiceStatus
    FROM SocialService;
END

