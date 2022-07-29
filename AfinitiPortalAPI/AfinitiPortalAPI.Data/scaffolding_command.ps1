# OP_SQL_SERVER - MySql - TEST 
Scaffold-DbContext -Connection "Server=10.32.21.165;Port=3004;database=portaldb;Uid=akay;Pwd=P@ssw0rd;AllowLoadLocalInfile=true;TreatTinyAsBoolean=True;" -Provider Pomelo.EntityFrameworkCore.MySql -Project AfinitiPortalAPI.Data -OutputDir "PortalDBContext\Entity" -ContextDir "PortalDBContext" -Context PortalDBContext -Force

# OP_SQL_SERVER - MySql - PROD 
Scaffold-DbContext -Connection "Server=10.32.21.164;Port=3003;database=portaldb;Uid=akay;Pwd=P@ssw0rd;AllowLoadLocalInfile=true;TreatTinyAsBoolean=True;" -Provider Pomelo.EntityFrameworkCore.MySql -Project AfinitiPortalAPI.Data -OutputDir "PortalDBContext\Entity" -ContextDir "PortalDBContext" -Context PortalDBContext -Force