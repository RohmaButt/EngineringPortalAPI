CREATE TABLE MONITORING_DB.ep.audit_logs (
	Id					bigint primary key identity,
	CreatedAt			datetime2 not null,
	ActionType			tinyint not null,
	Path				nvarchar(1000) null,
	ApiUrl				nvarchar(1000) null,
	DomSelector			nvarchar(1000) null,
	ClientIp			nvarchar(50) not null,
	ClientBrowser	    nvarchar(1000) null,
	Email				nvarchar(255) null,
	ActionCode			nvarchar(50) null ,
	Properties			nvarchar(max) null 
)
go

create index al_created_at_email_idx
	on  MONITORING_DB.ep.audit_logs (CreatedAt desc, Email)
go

create index al_created_at_idx
	on  MONITORING_DB.ep.audit_logs (CreatedAt desc)
go

------------------------------------------------------------------------------------------------------

CREATE TABLE  MONITORING_DB.ep.audit_actions (
	id					bigint primary key identity,
	created_at			datetime2 not null,
	action_code			nvarchar(50) null,
	description			nvarchar(max) null
)
go

------------------------------------------------------------------------------------------------------
