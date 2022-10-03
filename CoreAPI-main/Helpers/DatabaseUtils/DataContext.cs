using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Helpers.DatabaseUtils
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public virtual DbSet<VMenuUser> VMenuUsers { get; set; }
        public virtual DbSet<VMenu> VMenu { get; set; }
        public virtual DbSet<VRoleDetail> VRoleDetail { get; set; }
        public virtual DbSet<SForm> SForms { get; set; }
        public virtual DbSet<FastBank> FastBanks { get; set; }
        public virtual DbSet<IpcEmailtemplate> IpcEmailtemplates { get; set; }
        public virtual DbSet<IpcSchedule> IpcSchedules { get; set; }
        public virtual DbSet<SBank> SBanks { get; set; }
        public virtual DbSet<SBranch> SBranches { get; set; }
        public virtual DbSet<SDepartment> SDepartments { get; set; }
        public virtual DbSet<SSystemcode> SSystemcodes { get; set; }
        public virtual DbSet<SUserpolicy> SUserpolicies { get; set; }
        public virtual DbSet<SUserrole> SUserroles { get; set; }
        public virtual DbSet<SUsrac> SUsracs { get; set; }
        public virtual DbSet<VFastIncoming> VFastIncomings { get; set; }
        public virtual DbSet<VFastOutgoing> VFastOutgoings { get; set; }
        public virtual DbSet<SSearch> SSearches { get; set; }
        public virtual DbSet<SLoopkup> SLoopkups { get; set; }
        public virtual DbSet<SUserroledetail> SUserroledetails { get; set; }
        public virtual DbSet<SGetinfor> SGetinfors { get; set; }
        public virtual DbSet<SToken> STokens { get; set; }
        public virtual DbSet<Ipcoutputdefinexml> Ipcoutputdefinexmls { get; set; }
        public virtual DbSet<Ipclogdefine> Ipclogdefines { get; set; }
        public virtual DbSet<Ipclogtran> Ipclogtrans { get; set; }
        public virtual DbSet<SParam> SParams { get; set; }
        public virtual DbSet<SReport> SReports { get; set; }
        public virtual DbSet<SReportdata> SReportdatas { get; set; }
        public virtual DbSet<SReportparam> SReportparams { get; set; }
        public virtual DbSet<SFastaccount> SFastaccounts { get; set; }
        public virtual DbSet<SUserpublic> SUserpublics { get; set; }
        public virtual DbSet<Ipcmappingmsg> Ipcmappingmsgs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Ipcmappingmsg>(entity =>
            {
                entity.HasKey(e => new { e.fieldno, e.ipctrancode })
                    .HasName("PK__IPCMAPPI__BC5CAB6592B6F2D0");

                entity.ToTable("IPCMAPPINGMSG");

                entity.Property(e => e.fieldno).HasColumnName("FIELDNO");

                entity.Property(e => e.ipctrancode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IPCTRANCODE");

                entity.Property(e => e.defaultvalue)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DEFAULTVALUE");

                entity.Property(e => e.destfield)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESTFIELD");

                entity.Property(e => e.fieldformat)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("FIELDFORMAT");

                entity.Property(e => e.fieldtype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FIELDTYPE");

                entity.Property(e => e.sourcefield)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SOURCEFIELD");
            });

            modelBuilder.Entity<SFastaccount>(entity =>
            {
                entity.HasKey(e => e.accnum)
                    .HasName("PK__S_FASTAC__E87AAAD1C9D02F53");

                entity.ToTable("S_FASTACCOUNT");

                entity.Property(e => e.accnum)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("accnum");

                entity.Property(e => e.accname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("accname");

                entity.Property(e => e.balance)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("balance");

                entity.Property(e => e.branchnum)
                    .HasMaxLength(10)
                    .HasColumnName("branchnum");

                entity.Property(e => e.currency)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.opendate)
                    .HasColumnType("datetime")
                    .HasColumnName("opendate");

                entity.Property(e => e.status)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.type)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<SUserpublic>(entity =>
            {
                entity.HasKey(e => e.username)
                    .HasName("PK__S_USERPU__F3DBC573ACAA310C");

                entity.ToTable("S_USERPUBLIC");

                entity.Property(e => e.username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.passwordcore)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("passwordcore");

                entity.Property(e => e.usernamecore)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usernamecore");
            });

            modelBuilder.Entity<SReport>(entity =>
            {
                entity.HasKey(e => e.reportcode)
                    .HasName("PK__S_REPORT__027E8D0F29DBD513");

                entity.ToTable("S_REPORT");

                entity.Property(e => e.reportcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("reportcode")
                    .HasComment("");

                entity.Property(e => e.reportdesc)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("reportdesc");

                entity.Property(e => e.reportname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("reportname");
            });

            modelBuilder.Entity<SReportdata>(entity =>
            {
                entity.HasKey(e => new { e.spname, e.reportcode })
                    .HasName("PK__S_REPORT__8F782BFABF77BA4B");

                entity.ToTable("S_REPORTDATA");

                entity.Property(e => e.spname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("spname");

                entity.Property(e => e.reportcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("reportcode");

                entity.Property(e => e.description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.outputtype)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("outputtype");

                entity.Property(e => e.sourcetype)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sourcetype");
            });

            modelBuilder.Entity<SReportparam>(entity =>
            {
                entity.HasKey(e => new { e.paramname, e.spname })
                    .HasName("PK__S_REPORT__276849E19DB53CA7");

                entity.ToTable("S_REPORTPARAM");

                entity.Property(e => e.paramname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("paramname");

                entity.Property(e => e.spname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("spname");

                entity.Property(e => e.defaultvalue)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("defaultvalue");

                entity.Property(e => e.description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.paramtype)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("paramtype");
            });

            modelBuilder.Entity<SParam>(entity =>
            {
                entity.HasKey(e => e.parname);

                entity.ToTable("S_PARAM");

                entity.HasIndex(e => e.parcode, "IX_S_PARAM_CODE");

                entity.HasIndex(e => e.parcode, "IX_S_PARAM_GRP");

                entity.Property(e => e.parname)
                    .HasMaxLength(50)
                    .HasColumnName("parname");

                entity.Property(e => e.descr)
                    .HasMaxLength(250)
                    .HasColumnName("descr");

                entity.Property(e => e.mval).HasColumnName("mval");

                entity.Property(e => e.parcode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("parcode")
                    .IsFixedLength(true);

                entity.Property(e => e.pargrp)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("pargrp");

                entity.Property(e => e.parval).HasColumnName("parval");
            });

            modelBuilder.Entity<Ipclogdefine>(entity =>
            {
                entity.HasKey(e => new { e.ipctrancode, e.parmtype, e.pos, e.logtype })
                    .HasName("PK_IPCLOGDEFINE_1");

                entity.ToTable("IPCLOGDEFINE");

                entity.Property(e => e.ipctrancode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IPCTRANCODE");

                entity.Property(e => e.parmtype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PARMTYPE");

                entity.Property(e => e.pos).HasColumnName("POS");

                entity.Property(e => e.logtype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LOGTYPE");

                entity.Property(e => e.fieldname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FIELDNAME");
            });

            modelBuilder.Entity<Ipclogtran>(entity =>
            {
                entity.HasKey(e => new { e.ipcworkdate, e.ipctransid })
                    .HasName("PK_IPCLOGTRANS_1");

                entity.ToTable("IPCLOGTRANS");

                entity.HasIndex(e => e.tranref, "UQ__IPCLOGTR__D47668FADDCB0EFB")
                    .IsUnique();

                entity.Property(e => e.ipcworkdate)
                    .HasColumnType("datetime")
                    .HasColumnName("IPCWORKDATE");

                entity.Property(e => e.ipctransid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("IPCTRANSID");

                entity.Property(e => e.apprsts).HasColumnName("APPRSTS");

                entity.Property(e => e.batchref)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BATCHREF");

                entity.Property(e => e.ccyid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CCYID");

                entity.Property(e => e.char01)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR01");

                entity.Property(e => e.char02)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR02");

                entity.Property(e => e.char03)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR03");

                entity.Property(e => e.char04)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR04");

                entity.Property(e => e.char05)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR05");

                entity.Property(e => e.char06)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR06");

                entity.Property(e => e.char07)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR07");

                entity.Property(e => e.char08)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR08");

                entity.Property(e => e.char09)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR09");

                entity.Property(e => e.char10)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR10");

                entity.Property(e => e.char11)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR11");

                entity.Property(e => e.char12)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR12");

                entity.Property(e => e.char13)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR13");

                entity.Property(e => e.char14)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR14");

                entity.Property(e => e.char15)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR15");

                entity.Property(e => e.char16)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR16");

                entity.Property(e => e.char17)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR17");

                entity.Property(e => e.char18)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR18");

                entity.Property(e => e.char19)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR19");

                entity.Property(e => e.char20)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR20");

                entity.Property(e => e.char21)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR21");

                entity.Property(e => e.char22)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR22");

                entity.Property(e => e.char23)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR23");

                entity.Property(e => e.char24)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR24");

                entity.Property(e => e.char25)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR25");

                entity.Property(e => e.char26)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR26");

                entity.Property(e => e.char27)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR27");

                entity.Property(e => e.char28)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR28");

                entity.Property(e => e.char29)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR29");

                entity.Property(e => e.char30)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR30");

                entity.Property(e => e.char31)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR31");

                entity.Property(e => e.char32)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR32");

                entity.Property(e => e.char33)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR33");

                entity.Property(e => e.char34)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR34");

                entity.Property(e => e.char35)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR35");

                entity.Property(e => e.char36)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR36");

                entity.Property(e => e.char37)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR37");

                entity.Property(e => e.char38)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR38");

                entity.Property(e => e.char39)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR39");

                entity.Property(e => e.char40)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR40");

                entity.Property(e => e.char41)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR41");

                entity.Property(e => e.char42)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR42");

                entity.Property(e => e.char43)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR43");

                entity.Property(e => e.char44)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR44");

                entity.Property(e => e.char45)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR45");

                entity.Property(e => e.char46)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR46");

                entity.Property(e => e.char47)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR47");

                entity.Property(e => e.char48)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR48");

                entity.Property(e => e.char49)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR49");

                entity.Property(e => e.char50)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR50");

                entity.Property(e => e.char51)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR51");

                entity.Property(e => e.char52)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR52");

                entity.Property(e => e.char53)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR53");

                entity.Property(e => e.char54)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR54");

                entity.Property(e => e.char55)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR55");

                entity.Property(e => e.char56)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR56");

                entity.Property(e => e.char57)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR57");

                entity.Property(e => e.char58)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR58");

                entity.Property(e => e.char59)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR59");

                entity.Property(e => e.char60)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR60");

                entity.Property(e => e.char61)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR61");

                entity.Property(e => e.char62)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR62");

                entity.Property(e => e.char63)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR63");

                entity.Property(e => e.char64)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR64");

                entity.Property(e => e.char65)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR65");

                entity.Property(e => e.char66)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR66");

                entity.Property(e => e.char67)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR67");

                entity.Property(e => e.char68)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR68");

                entity.Property(e => e.char69)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR69");

                entity.Property(e => e.char70)
                    .HasMaxLength(1000)
                    .HasColumnName("CHAR70");

                entity.Property(e => e.deleted).HasColumnName("DELETED");

                entity.Property(e => e.desterrorcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESTERRORCODE");

                entity.Property(e => e.destid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DESTID");

                entity.Property(e => e.desttranref)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DESTTRANREF");

                entity.Property(e => e.errorcode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ERRORCODE")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.errordesc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("ERRORDESC")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ipctrancode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IPCTRANCODE");

                entity.Property(e => e.ipctransdate)
                    .HasColumnType("datetime")
                    .HasColumnName("IPCTRANSDATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.isbatch)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ISBATCH");

                entity.Property(e => e.listuserapp)
                    .IsUnicode(false)
                    .HasColumnName("LISTUSERAPP");

                entity.Property(e => e.listuserprs)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("LISTUSERPRS");

                entity.Property(e => e.nextuserapp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NEXTUSERAPP");

                entity.Property(e => e.num01)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM01");

                entity.Property(e => e.num02)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM02");

                entity.Property(e => e.num03)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM03");

                entity.Property(e => e.num04)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM04");

                entity.Property(e => e.num05)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM05");

                entity.Property(e => e.num06)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM06");

                entity.Property(e => e.num07)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM07");

                entity.Property(e => e.num08)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM08");

                entity.Property(e => e.num09)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM09");

                entity.Property(e => e.num10)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM10");

                entity.Property(e => e.num11)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM11");

                entity.Property(e => e.num12)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM12");

                entity.Property(e => e.num13)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM13");

                entity.Property(e => e.num14)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM14");

                entity.Property(e => e.num15)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM15");

                entity.Property(e => e.num16)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM16");

                entity.Property(e => e.num17)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM17");

                entity.Property(e => e.num18)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM18");

                entity.Property(e => e.num19)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM19");

                entity.Property(e => e.num20)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM20");

                entity.Property(e => e.num21)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM21");

                entity.Property(e => e.num22)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM22");

                entity.Property(e => e.num23)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM23");

                entity.Property(e => e.num24)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM24");

                entity.Property(e => e.num25)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM25");

                entity.Property(e => e.num26)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM26");

                entity.Property(e => e.num27)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM27");

                entity.Property(e => e.num28)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM28");

                entity.Property(e => e.num29)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM29");

                entity.Property(e => e.num30)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM30");

                entity.Property(e => e.num31)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM31");

                entity.Property(e => e.num32)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM32");

                entity.Property(e => e.num33)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM33");

                entity.Property(e => e.num34)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM34");

                entity.Property(e => e.num35)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM35");

                entity.Property(e => e.num36)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM36");

                entity.Property(e => e.num37)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM37");

                entity.Property(e => e.num38)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM38");

                entity.Property(e => e.num39)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM39");

                entity.Property(e => e.num40)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM40");

                entity.Property(e => e.num41)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM41");

                entity.Property(e => e.num42)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM42");

                entity.Property(e => e.num43)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM43");

                entity.Property(e => e.num44)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM44");

                entity.Property(e => e.num45)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM45");

                entity.Property(e => e.num46)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM46");

                entity.Property(e => e.num47)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM47");

                entity.Property(e => e.num48)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM48");

                entity.Property(e => e.num49)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM49");

                entity.Property(e => e.num50)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM50");

                entity.Property(e => e.num51)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM51");

                entity.Property(e => e.num52)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM52");

                entity.Property(e => e.num53)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM53");

                entity.Property(e => e.num54)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM54");

                entity.Property(e => e.num55)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM55");

                entity.Property(e => e.num56)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM56");

                entity.Property(e => e.num57)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM57");

                entity.Property(e => e.num58)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM58");

                entity.Property(e => e.num59)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM59");

                entity.Property(e => e.num60)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM60");

                entity.Property(e => e.num61)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM61");

                entity.Property(e => e.num62)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM62");

                entity.Property(e => e.num63)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM63");

                entity.Property(e => e.num64)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM64");

                entity.Property(e => e.num65)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM65");

                entity.Property(e => e.num66)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM66");

                entity.Property(e => e.num67)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM67");

                entity.Property(e => e.num68)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM68");

                entity.Property(e => e.num69)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM69");

                entity.Property(e => e.num70)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("NUM70");

                entity.Property(e => e.offlsts)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("OFFLSTS");

                entity.Property(e => e.online)
                    .IsRequired()
                    .HasColumnName("ONLINE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.receiverbank)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RECEIVERBANK");

                entity.Property(e => e.receiverchannel)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RECEIVERCHANNEL");

                entity.Property(e => e.senderbank)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SENDERBANK");

                entity.Property(e => e.senderchannel)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SENDERCHANNEL");

                entity.Property(e => e.sourceid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SOURCEID");

                entity.Property(e => e.sourcetranref)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SOURCETRANREF");

                entity.Property(e => e.status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.statushis)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("STATUSHIS");

                entity.Property(e => e.trandesc)
                    .IsRequired()
                    .HasMaxLength(400)
                    .HasColumnName("TRANDESC");

                entity.Property(e => e.tranref)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TRANREF");

                entity.Property(e => e.usercurapp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USERCURAPP");

                entity.Property(e => e.userid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USERID");
            });

            modelBuilder.Entity<Ipcoutputdefinexml>(entity =>
            {
                entity.HasKey(e => new { e.sourceid, e.destid, e.ipctrancode, e.online, e.fieldno });

                entity.ToTable("IPCOUTPUTDEFINEXML");

                entity.Property(e => e.sourceid)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SOURCEID");

                entity.Property(e => e.destid)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DESTID");

                entity.Property(e => e.ipctrancode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IPCTRANCODE");

                entity.Property(e => e.online).HasColumnName("ONLINE");

                entity.Property(e => e.fieldno).HasColumnName("FIELDNO");

                entity.Property(e => e.arraylevel).HasColumnName("ARRAYLEVEL");

                entity.Property(e => e.arraynode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ARRAYNODE");

                entity.Property(e => e.fielddesc)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FIELDDESC");

                entity.Property(e => e.fieldname)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("FIELDNAME");

                entity.Property(e => e.formatobject)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("FORMATOBJECT");

                entity.Property(e => e.fieldstyle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FIELDSTYLE");

                entity.Property(e => e.formatfunction)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("FORMATFUNCTION");

                entity.Property(e => e.formatobject)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("FORMATOBJECT");

                entity.Property(e => e.formatparm)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("FORMATPARM");

                entity.Property(e => e.formattype)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FORMATTYPE");

                entity.Property(e => e.sublevel).HasColumnName("SUBLEVEL");

                entity.Property(e => e.valuename)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("VALUENAME");

                entity.Property(e => e.valuestyle)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("VALUESTYLE");

                entity.Property(e => e.defaultvalue)
                    .HasMaxLength(1000)
                    .HasColumnName("DEFAULTVALUE");
            });

            modelBuilder.Entity<SToken>(entity =>
            {
                entity.HasKey(e => new { e.bankid, e.varname, e.varext });

                entity.ToTable("S_TOKEN");

                entity.Property(e => e.bankid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BANKID");

                entity.Property(e => e.varname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("VARNAME");

                entity.Property(e => e.varext)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("VAREXT");

                entity.Property(e => e.description)
                    .HasMaxLength(255)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.vardate)
                    .HasColumnType("datetime")
                    .HasColumnName("VARDATE");

                entity.Property(e => e.varvalue)
                    .HasMaxLength(4000)
                    .HasColumnName("VARVALUE");
            });

            modelBuilder.Entity<SGetinfor>(entity =>
            {
                entity.HasKey(e => new { e.inforid, e.key })
                    .HasName("PK__S_GETINF__3EDD04090148B2EF");

                entity.ToTable("S_GETINFOR");

                entity.Property(e => e.inforid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("inforid");

                entity.Property(e => e.key)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("key");

                entity.Property(e => e.type)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.value)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("value");
            });

            modelBuilder.Entity<SUserroledetail>(entity =>
            {
                entity.HasKey(e => new { e.usrname, e.roleid });

                entity.ToTable("S_USERROLEDETAIL");

                entity.Property(e => e.usrname)
                    .HasMaxLength(50)
                    .HasColumnName("usrname");

                entity.Property(e => e.roleid).HasColumnName("roleid");

                entity.Property(e => e.description).HasColumnName("description");
            });

            modelBuilder.Entity<SLoopkup>(entity =>
            {
                entity.HasKey(e => new { e.lookupid, e.tablename, e.key, e.colunmname })
                    .HasName("PK__S_LOOPKU__7F2610539D9EB68B");

                entity.ToTable("S_LOOPKUP");

                entity.Property(e => e.lookupid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LOOKUPID");

                entity.Property(e => e.tablename)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TABLENAME");

                entity.Property(e => e.key)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("KEY");

                entity.Property(e => e.colunmname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("COLUNMNAME");

                entity.Property(e => e.query)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("QUERY");
            });

            modelBuilder.Entity<VMenuUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_MENU_USER");

                entity.Property(e => e.actionid)
                    .HasMaxLength(10)
                    .HasColumnName("ACTIONID");

                entity.Property(e => e.actionsetting)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("ACTIONSETTING");

                entity.Property(e => e.caption)
                    .HasMaxLength(1000)
                    .HasColumnName("CAPTION");

                entity.Property(e => e.cmdtype)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CMDTYPE");

                entity.Property(e => e.icon)
                    .HasMaxLength(50)
                    .HasColumnName("ICON");

                entity.Property(e => e.menuid)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MENUID");

                entity.Property(e => e.menuorder).HasColumnName("MENUORDER");

                entity.Property(e => e.menuparent)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MENUPARENT");

                entity.Property(e => e.menupath)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("MENUPATH");

                entity.Property(e => e.pageid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PAGEID");

                entity.Property(e => e.searchfunc)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SEARCHFUNC");

                entity.Property(e => e.type)
                    .HasMaxLength(10)
                    .HasColumnName("TYPE");

                entity.Property(e => e.username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("USERNAME");
            });

            modelBuilder.Entity<VMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_MENU");

                entity.Property(e => e.caption)
                    .HasMaxLength(1000)
                    .HasColumnName("CAPTION");

                entity.Property(e => e.cmdtype)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CMDTYPE");

                entity.Property(e => e.icon)
                    .HasMaxLength(50)
                    .HasColumnName("ICON");

                entity.Property(e => e.menuid)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MENUID");

                entity.Property(e => e.menuorder).HasColumnName("MENUORDER");

                entity.Property(e => e.menuparent)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MENUPARENT");

                entity.Property(e => e.menupath)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("MENUPATH");

                entity.Property(e => e.type)
                    .HasMaxLength(10)
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<VRoleDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ROLEDETAIL");
                entity.Property(e => e.menuid)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MENUID");
                entity.Property(e => e.roleid)
                    .IsRequired()
                    .HasColumnName("ROLEID");
                entity.Property(e => e.invoke)
                    .IsRequired()
                    .HasColumnName("INVOKE");
            });

            modelBuilder.Entity<SForm>(entity =>
            {
                entity.HasKey(e => new { e.formname, e.formtag });

                entity.ToTable("S_FORM");

                entity.Property(e => e.formname)
                    .HasMaxLength(20)
                    .HasColumnName("formname");

                entity.Property(e => e.formtag)
                    .HasMaxLength(50)
                    .HasColumnName("formtag");

                entity.Property(e => e.afterspan).HasColumnName("afterspan");

                entity.Property(e => e.beforespan).HasColumnName("beforespan");

                entity.Property(e => e.caption)
                    .HasMaxLength(4000)
                    .HasColumnName("caption");

                entity.Property(e => e.colunm).HasColumnName("colunm");

                entity.Property(e => e.controlaction)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("controlaction");

                entity.Property(e => e.defaultvalue)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("defaultvalue");

                entity.Property(e => e.format)
                    .HasMaxLength(50)
                    .HasColumnName("format");

                entity.Property(e => e.formuid)
                    .HasMaxLength(50)
                    .HasColumnName("formuid");

                entity.Property(e => e.isadd).HasColumnName("isadd");

                entity.Property(e => e.ismodify).HasColumnName("ismodify");

                entity.Property(e => e.isreadonly).HasColumnName("isreadonly");

                entity.Property(e => e.isrequired).HasColumnName("isrequired");

                entity.Property(e => e.isvisible).HasColumnName("isvisible");

                entity.Property(e => e.isvisibleonadd).HasColumnName("isvisibleonadd");

                entity.Property(e => e.line).HasColumnName("line");

                entity.Property(e => e.order).HasColumnName("order");
                entity.Property(e => e.ctltype).HasColumnName("ctltype");
                entity.Property(e => e.prefield)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("prefield");

                entity.Property(e => e.type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("type");

                entity.Property(e => e.validation)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("validation");
            });

            modelBuilder.Entity<SSearch>(entity =>
            {
                entity.HasKey(e => new { e.searchfunc, e.ftag });

                entity.ToTable("S_SEARCH");

                entity.Property(e => e.searchfunc)
                    .HasMaxLength(50)
                    .HasColumnName("SEARCHFUNC");

                entity.Property(e => e.ftag)
                    .HasMaxLength(50)
                    .HasColumnName("FTAG");

                entity.Property(e => e.caption)
                    .IsRequired()
                    .HasColumnName("CAPTION");

                entity.Property(e => e.isadvsearch).HasColumnName("ISADVSEARCH");

                entity.Property(e => e.isorder).HasColumnName("ISORDER");

                entity.Property(e => e.ispk).HasColumnName("ISPK");

                entity.Property(e => e.isvisible).HasColumnName("ISVISIBLE");

                entity.Property(e => e.order).HasColumnName("ORDER");

                entity.Property(e => e.searchname)
                    .IsRequired()
                    .HasColumnName("SEARCHNAME");

                entity.Property(e => e.soption)
                    .HasMaxLength(10)
                    .HasColumnName("SOPTION");

                entity.Property(e => e.type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TYPE");

                entity.Property(e => e.uid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("UID");

                entity.Property(e => e.width)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("WIDTH");
            });

            modelBuilder.Entity<FastBank>(entity =>
            {
                entity.HasKey(e => e.bankid);

                entity.ToTable("FAST_BANK");

                entity.Property(e => e.bankid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("bankid");

                entity.Property(e => e.bankbuilding)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("bankbuilding");

                entity.Property(e => e.bankcountry)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("bankcountry");

                entity.Property(e => e.bankname)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("bankname");

                entity.Property(e => e.bankpostalcode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("bankpostalcode");

                entity.Property(e => e.bankstreet)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("bankstreet");

                entity.Property(e => e.banktown)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("banktown");

                entity.Property(e => e.participatecode)
                    .IsRequired()
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("participatecode");

                entity.Property(e => e.bicfi)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("bicfi");
            });

            modelBuilder.Entity<IpcEmailtemplate>(entity =>
            {
                entity.HasKey(e => new { e.bankid, e.channelid, e.trancode });

                entity.ToTable("IPC_EMAILTEMPLATE");

                entity.Property(e => e.bankid).HasColumnName("BANKID");

                entity.Property(e => e.channelid)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CHANNELID");

                entity.Property(e => e.trancode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TRANCODE");

                entity.Property(e => e.attachment).HasColumnName("ATTACHMENT");

                entity.Property(e => e.attachmentpassword)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ATTACHMENTPASSWORD")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.content).HasColumnName("CONTENT");

                entity.Property(e => e.contentpassword)
                    .HasColumnName("CONTENTPASSWORD")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.from)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("FROM");

                entity.Property(e => e.lang)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LANG");

                entity.Property(e => e.rowdetailtable)
                    .IsUnicode(false)
                    .HasColumnName("ROWDETAILTABLE")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.rowdetailtableatachment)
                    .IsUnicode(false)
                    .HasColumnName("ROWDETAILTABLEATACHMENT")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.sendattachment)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SENDATTACHMENT");

                entity.Property(e => e.serviceid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SERVICEID");

                entity.Property(e => e.title)
                    .HasMaxLength(1000)
                    .HasColumnName("TITLE");

                entity.Property(e => e.titlepassword)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("TITLEPASSWORD")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<IpcSchedule>(entity =>
            {
                entity.HasKey(e => e.scheduleid);

                entity.ToTable("IPC_SCHEDULES");

                entity.Property(e => e.scheduleid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SCHEDULEID");

                entity.Property(e => e.actionid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ACTIONID");

                entity.Property(e => e.actiontype)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ACTIONTYPE");

                entity.Property(e => e.createdate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATEDATE");

                entity.Property(e => e.description).HasColumnName("DESCRIPTION");

                entity.Property(e => e.enddate)
                    .HasColumnType("datetime")
                    .HasColumnName("ENDDATE");

                entity.Property(e => e.isapproved)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ISAPPROVED");

                entity.Property(e => e.nextexecute)
                    .HasColumnType("datetime")
                    .HasColumnName("NEXTEXECUTE");

                entity.Property(e => e.ord)
                    .HasColumnName("ORD")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.schedulename)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("SCHEDULENAME");

                entity.Property(e => e.scheduletime)
                    .HasColumnType("datetime")
                    .HasColumnName("SCHEDULETIME");

                entity.Property(e => e.scheduletype)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SCHEDULETYPE");

                entity.Property(e => e.serviceid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SERVICEID");

                entity.Property(e => e.status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.userapproved)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USERAPPROVED");

                entity.Property(e => e.usercreate)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USERCREATE");
            });

            modelBuilder.Entity<SBank>(entity =>
            {
                entity.HasKey(e => e.bankid);

                entity.ToTable("S_BANK");

                entity.Property(e => e.bankid).HasColumnName("bankid");

                entity.Property(e => e.autoacceptquote)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("autoacceptquote")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.autogetquote)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("autogetquote")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.bankcode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bankcode");

                entity.Property(e => e.bankname)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("bankname");

                entity.Property(e => e.banksts)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("banksts");

                entity.Property(e => e.countrycode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("countrycode");

                entity.Property(e => e.datecreated)
                    .HasColumnType("datetime")
                    .HasColumnName("datecreated");

                entity.Property(e => e.datemodified)
                    .HasColumnType("datetime")
                    .HasColumnName("datemodified");

                entity.Property(e => e.issender)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("issender")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.mt103acceptpath)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("mt103acceptpath");

                entity.Property(e => e.mt103rejectpath)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("mt103rejectpath");

                entity.Property(e => e.serviceid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("serviceid")
                    .HasDefaultValueSql("('BLS')");

                entity.Property(e => e.uri)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("uri");

                entity.Property(e => e.usercreated)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usercreated");

                entity.Property(e => e.usermodified)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usermodified");
            });

            modelBuilder.Entity<SBranch>(entity =>
            {
                entity.HasKey(e => e.branchid);

                entity.ToTable("S_BRANCH");

                entity.Property(e => e.branchid)
                    .ValueGeneratedNever()
                    .HasColumnName("BRANCHID");

                entity.Property(e => e.address)
                    .HasMaxLength(500)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.bcycid)
                    .HasMaxLength(3)
                    .HasColumnName("BCYCID")
                    .IsFixedLength(true);

                entity.Property(e => e.bcynm)
                    .HasMaxLength(3)
                    .HasColumnName("BCYNM")
                    .IsFixedLength(true);

                entity.Property(e => e.branchcd)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("BRANCHCD");

                entity.Property(e => e.brname)
                    .HasMaxLength(500)
                    .HasColumnName("BRNAME");

                entity.Property(e => e.country)
                    .HasMaxLength(2)
                    .HasColumnName("COUNTRY")
                    .IsFixedLength(true);

                entity.Property(e => e.datefmt)
                    .HasMaxLength(10)
                    .HasColumnName("DATEFMT");

                entity.Property(e => e.isonline)
                    .HasMaxLength(1)
                    .HasColumnName("ISONLINE")
                    .IsFixedLength(true);

                entity.Property(e => e.lang)
                    .HasMaxLength(2)
                    .HasColumnName("LANG")
                    .IsFixedLength(true);

                entity.Property(e => e.lcycid)
                    .HasMaxLength(3)
                    .HasColumnName("LCYCID")
                    .IsFixedLength(true);

                entity.Property(e => e.lcynm)
                    .HasMaxLength(3)
                    .HasColumnName("LCYNM")
                    .IsFixedLength(true);

                entity.Property(e => e.ldatefmt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("LDATEFMT");

                entity.Property(e => e.mphone)
                    .HasMaxLength(255)
                    .HasColumnName("MPHONE");

                entity.Property(e => e.numfmtd)
                    .HasMaxLength(1)
                    .HasColumnName("NUMFMTD")
                    .IsFixedLength(true);

                entity.Property(e => e.numfmtt)
                    .HasMaxLength(1)
                    .HasColumnName("NUMFMTT")
                    .IsFixedLength(true);

                entity.Property(e => e.phone)
                    .HasMaxLength(255)
                    .HasColumnName("PHONE");

                entity.Property(e => e.refcode)
                    .HasMaxLength(250)
                    .HasColumnName("REFCODE");

                entity.Property(e => e.refid)
                    .HasMaxLength(15)
                    .HasColumnName("REFID");

                entity.Property(e => e.taxcode)
                    .HasMaxLength(50)
                    .HasColumnName("TAXCODE");

                entity.Property(e => e.timefmt)
                    .HasMaxLength(10)
                    .HasColumnName("TIMEFMT");

                entity.Property(e => e.timezn).HasColumnName("TIMEZN");

                entity.Property(e => e.udfield1).HasColumnName("UDFIELD1");
            });

            modelBuilder.Entity<SDepartment>(entity =>
            {
                entity.HasKey(e => e.deprtid);

                entity.ToTable("S_DEPARTMENT");

                entity.Property(e => e.deprtid)
                    .ValueGeneratedNever()
                    .HasColumnName("DEPRTID");

                entity.Property(e => e.branchid).HasColumnName("BRANCHID");

                entity.Property(e => e.deprname).HasColumnName("DEPRNAME");

                entity.Property(e => e.deprtcd)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("DEPRTCD");
            });

            modelBuilder.Entity<SSystemcode>(entity =>
            {
                entity.HasKey(e => new { e.cdid, e.cdname, e.cdgrp })
                    .HasName("PK_S_SYSTEMCODE_CDID");

                entity.ToTable("S_SYSTEMCODE");

                entity.Property(e => e.cdid)
                    .HasMaxLength(20)
                    .HasColumnName("cdid");

                entity.Property(e => e.cdname)
                    .HasMaxLength(50)
                    .HasColumnName("cdname");

                entity.Property(e => e.cdgrp)
                    .HasMaxLength(10)
                    .HasColumnName("cdgrp")
                    .IsFixedLength(true);

                entity.Property(e => e.caption)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("caption");

                entity.Property(e => e.cdidx).HasColumnName("cdidx");

                entity.Property(e => e.isvisible).HasColumnName("isvisible");
            });

            modelBuilder.Entity<SUserpolicy>(entity =>
            {
                entity.HasKey(e => e.policyid)
                    .HasName("pk_policy");

                entity.ToTable("S_USERPOLICY");

                entity.Property(e => e.policyid)
                    .ValueGeneratedNever()
                    .HasColumnName("policyid");

                entity.Property(e => e.baseonpolicy).HasColumnName("baseonpolicy");

                entity.Property(e => e.contractid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("contractid");

                entity.Property(e => e.datecreate)
                    .HasColumnType("datetime")
                    .HasColumnName("datecreate");

                entity.Property(e => e.datemodify)
                    .HasColumnType("datetime")
                    .HasColumnName("datemodify");

                entity.Property(e => e.descr)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("descr");

                entity.Property(e => e.effrom)
                    .HasColumnType("datetime")
                    .HasColumnName("effrom");

                entity.Property(e => e.efto)
                    .HasColumnType("datetime")
                    .HasColumnName("efto");

                entity.Property(e => e.isbankedit).HasColumnName("isbankedit");

                entity.Property(e => e.iscorpedit).HasColumnName("iscorpedit");

                entity.Property(e => e.isdefault).HasColumnName("isdefault");

                entity.Property(e => e.lginfr)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lginfr");

                entity.Property(e => e.lginto)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lginto");

                entity.Property(e => e.llkoutthrs)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("llkoutthrs");

                entity.Property(e => e.minpwdlen).HasColumnName("minpwdlen");

                entity.Property(e => e.pwccplxsn).HasColumnName("pwccplxsn");

                entity.Property(e => e.pwdagemax).HasColumnName("pwdagemax");

                entity.Property(e => e.pwdcplx).HasColumnName("pwdcplx");

                entity.Property(e => e.pwdcplxlc).HasColumnName("pwdcplxlc");

                entity.Property(e => e.pwdcplxsc).HasColumnName("pwdcplxsc");

                entity.Property(e => e.pwdcplxuc).HasColumnName("pwdcplxuc");

                entity.Property(e => e.pwdft)
                    .HasMaxLength(100)
                    .HasColumnName("pwdft");

                entity.Property(e => e.pwdhis).HasColumnName("pwdhis");

                entity.Property(e => e.resetlkout)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("resetlkout");

                entity.Property(e => e.timelginrequire).HasColumnName("timelginrequire");

                entity.Property(e => e.usercreate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usercreate");

                entity.Property(e => e.usermodify)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usermodify");
            });


            modelBuilder.Entity<SUserrole>(entity =>
            {
                entity.HasKey(e => e.roleid)
                    .HasName("pk_s_userrole");

                entity.ToTable("S_USERROLE");

                entity.Property(e => e.roleid).HasColumnName("roleid");

                entity.Property(e => e.contractno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("contractno");

                entity.Property(e => e.datecreated)
                    .HasColumnType("datetime")
                    .HasColumnName("datecreated");

                entity.Property(e => e.datemodified)
                    .HasColumnType("datetime")
                    .HasColumnName("datemodified");

                entity.Property(e => e.isshow)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("isshow");

                entity.Property(e => e.roledescription)
                    .HasMaxLength(4000)
                    .HasColumnName("roledescription");

                entity.Property(e => e.rolename)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("rolename");

                entity.Property(e => e.serviceid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("serviceid");

                entity.Property(e => e.status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.usercreated)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usercreated");

                entity.Property(e => e.usermodified)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usermodified");

                entity.Property(e => e.usertype)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usertype");
            });

            modelBuilder.Entity<SUsrac>(entity =>
            {
                entity.HasKey(e => e.username);

                entity.ToTable("S_USRAC");

                entity.Property(e => e.username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.address)
                    .HasMaxLength(4000)
                    .HasColumnName("address");

                entity.Property(e => e.birthday)
                    .HasColumnType("datetime")
                    .HasColumnName("birthday");

                entity.Property(e => e.branchid).HasColumnName("branchid");

                entity.Property(e => e.corelgname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("corelgname");

                entity.Property(e => e.coreuserid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("coreuserid");

                entity.Property(e => e.datecreated)
                    .HasColumnType("datetime")
                    .HasColumnName("datecreated");

                entity.Property(e => e.datemodified)
                    .HasColumnType("datetime")
                    .HasColumnName("datemodified");

                entity.Property(e => e.deptid).HasColumnName("deptid");

                entity.Property(e => e.email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.expiretime)
                    .HasColumnType("datetime")
                    .HasColumnName("expiretime");

                entity.Property(e => e.failnumber).HasColumnName("failnumber");

                entity.Property(e => e.firstname)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("firstname");

                entity.Property(e => e.gender).HasColumnName("gender");

                entity.Property(e => e.islogin).HasColumnName("islogin");

                entity.Property(e => e.isshow).HasColumnName("isshow");

                entity.Property(e => e.lastlogintime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastlogintime");

                entity.Property(e => e.lastname)
                    .HasMaxLength(1000)
                    .HasColumnName("lastname");

                entity.Property(e => e.password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.phone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.policyid).HasColumnName("policyid");

                entity.Property(e => e.productid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("productid");

                entity.Property(e => e.status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.usercreated)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usercreated");

                entity.Property(e => e.userlevel).HasColumnName("userlevel");

                entity.Property(e => e.usermodified)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usermodified");

                entity.Property(e => e.usrid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("usrid");
            });

            modelBuilder.Entity<VFastIncoming>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_FAST_INCOMING");

                entity.Property(e => e.bacthbookg)
                    .HasMaxLength(1000)
                    .HasColumnName("BACTHBOOKG");

                entity.Property(e => e.chrgbr)
                    .HasMaxLength(1000)
                    .HasColumnName("CHRGBR");

                entity.Property(e => e.corests)
                    .HasMaxLength(1000)
                    .HasColumnName("CORESTS");

                entity.Property(e => e.corestsud)
                    .HasMaxLength(1000)
                    .HasColumnName("CORESTSUD");

                entity.Property(e => e.createdt)
                    .HasMaxLength(1000)
                    .HasColumnName("CREATEDT");

                entity.Property(e => e.endtoendid)
                    .HasMaxLength(1000)
                    .HasColumnName("ENDTOENDID");

                entity.Property(e => e.fcnsts)
                    .HasMaxLength(1000)
                    .HasColumnName("FCNSTS");

                entity.Property(e => e.fcnstsud)
                    .HasMaxLength(1000)
                    .HasColumnName("FCNSTSUD");

                entity.Property(e => e.fcntxrefid)
                    .HasMaxLength(1000)
                    .HasColumnName("FCNTXREFID");

                entity.Property(e => e.instrid)
                    .HasMaxLength(1000)
                    .HasColumnName("INSTRID");

                entity.Property(e => e.ipctransdate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IPCTRANSDATE");

                entity.Property(e => e.ipctransid).HasColumnName("IPCTRANSID");

                entity.Property(e => e.msgid)
                    .HasMaxLength(1000)
                    .HasColumnName("MSGID");

                entity.Property(e => e.status)
                    .HasMaxLength(63)
                    .HasColumnName("STATUS");

                entity.Property(e => e.pmtinfid)
                    .HasMaxLength(1000)
                    .HasColumnName("PMTINFID");

                entity.Property(e => e.pmtmtd)
                    .HasMaxLength(1000)
                    .HasColumnName("PMTMTD");

                entity.Property(e => e.purpose)
                    .HasMaxLength(1000)
                    .HasColumnName("PURPOSE");

                entity.Property(e => e.raccno)
                    .HasMaxLength(1000)
                    .HasColumnName("RACCNO");

                entity.Property(e => e.ramt)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("RAMT");

                entity.Property(e => e.rbank)
                    .HasMaxLength(1000)
                    .HasColumnName("RBANK");

                entity.Property(e => e.rbname)
                    .HasMaxLength(1000)
                    .HasColumnName("RBNAME");

                entity.Property(e => e.rbtown)
                    .HasMaxLength(1000)
                    .HasColumnName("RBTOWN");

                entity.Property(e => e.rcbdmb)
                    .HasMaxLength(1000)
                    .HasColumnName("RCBDMB");

                entity.Property(e => e.rccountry)
                    .HasMaxLength(1000)
                    .HasColumnName("RCCOUNTRY");

                entity.Property(e => e.rccr)
                    .HasMaxLength(1000)
                    .HasColumnName("RCCR");

                entity.Property(e => e.rcname)
                    .HasMaxLength(1000)
                    .HasColumnName("RCNAME");

                entity.Property(e => e.rcstreet)
                    .HasMaxLength(1000)
                    .HasColumnName("RCSTREET");

                entity.Property(e => e.rctown)
                    .HasMaxLength(1000)
                    .HasColumnName("RCTOWN");

                entity.Property(e => e.refcd)
                    .HasMaxLength(1000)
                    .HasColumnName("REFCD");

                entity.Property(e => e.refnb)
                    .HasMaxLength(1000)
                    .HasColumnName("REFNB");

                entity.Property(e => e.requestdt)
                    .HasMaxLength(1000)
                    .HasColumnName("REQUESTDT");

                entity.Property(e => e.saccno)
                    .HasMaxLength(1000)
                    .HasColumnName("SACCNO");

                entity.Property(e => e.samt)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("SAMT");

                entity.Property(e => e.sbank)
                    .HasMaxLength(1000)
                    .HasColumnName("SBANK");

                entity.Property(e => e.sbbdmb)
                    .HasMaxLength(1000)
                    .HasColumnName("SBBDMB");

                entity.Property(e => e.sbcountry)
                    .HasMaxLength(1000)
                    .HasColumnName("SBCOUNTRY");

                entity.Property(e => e.sbname)
                    .HasMaxLength(1000)
                    .HasColumnName("SBNAME");

                entity.Property(e => e.sbpcode)
                    .HasMaxLength(1000)
                    .HasColumnName("SBPCODE");

                entity.Property(e => e.sbstreet)
                    .HasMaxLength(1000)
                    .HasColumnName("SBSTREET");

                entity.Property(e => e.sbtown)
                    .HasMaxLength(1000)
                    .HasColumnName("SBTOWN");

                entity.Property(e => e.sccr)
                    .HasMaxLength(1000)
                    .HasColumnName("SCCR");

                entity.Property(e => e.sdbdmb)
                    .HasMaxLength(1000)
                    .HasColumnName("SDBDMB");

                entity.Property(e => e.sdcountry)
                    .HasMaxLength(1000)
                    .HasColumnName("SDCOUNTRY");

                entity.Property(e => e.sdname)
                    .HasMaxLength(1000)
                    .HasColumnName("SDNAME");

                entity.Property(e => e.sdpcode)
                    .HasMaxLength(1000)
                    .HasColumnName("SDPCODE");

                entity.Property(e => e.sdstreet)
                    .HasMaxLength(1000)
                    .HasColumnName("SDSTREET");

                entity.Property(e => e.sdtown)
                    .HasMaxLength(1000)
                    .HasColumnName("SDTOWN");

                entity.Property(e => e.strinfor)
                    .HasMaxLength(1000)
                    .HasColumnName("STRINFOR");

                entity.Property(e => e.tranref)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TRANREF");

                entity.Property(e => e.txrefid)
                    .HasMaxLength(1000)
                    .HasColumnName("TXREFID");
            });

            modelBuilder.Entity<VFastOutgoing>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_FAST_OUTGOING");

                entity.Property(e => e.bacthbookg)
                    .HasMaxLength(1000)
                    .HasColumnName("BACTHBOOKG");

                entity.Property(e => e.chrgbr)
                    .HasMaxLength(1000)
                    .HasColumnName("CHRGBR");

                entity.Property(e => e.corests)
                    .HasMaxLength(1000)
                    .HasColumnName("CORESTS");

                entity.Property(e => e.corestsud)
                    .HasMaxLength(1000)
                    .HasColumnName("CORESTSUD");

                entity.Property(e => e.createdt)
                    .HasMaxLength(1000)
                    .HasColumnName("CREATEDT");

                entity.Property(e => e.endtoendid)
                    .HasMaxLength(1000)
                    .HasColumnName("ENDTOENDID");

                entity.Property(e => e.fcnsts)
                    .HasMaxLength(1000)
                    .HasColumnName("FCNSTS");

                entity.Property(e => e.fcnstsud)
                    .HasMaxLength(1000)
                    .HasColumnName("FCNSTSUD");

                entity.Property(e => e.fcntxrefid)
                    .HasMaxLength(1000)
                    .HasColumnName("FCNTXREFID");

                entity.Property(e => e.instrid)
                    .HasMaxLength(1000)
                    .HasColumnName("INSTRID");

                entity.Property(e => e.ipctransdate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IPCTRANSDATE");

                entity.Property(e => e.ipctransid).HasColumnName("IPCTRANSID");

                entity.Property(e => e.msgid)
                    .HasMaxLength(1000)
                    .HasColumnName("MSGID");

                entity.Property(e => e.status)
                    .HasMaxLength(63)
                    .HasColumnName("STATUS");

                entity.Property(e => e.pmtinfid)
                    .HasMaxLength(1000)
                    .HasColumnName("PMTINFID");

                entity.Property(e => e.pmtmtd)
                    .HasMaxLength(1000)
                    .HasColumnName("PMTMTD");

                entity.Property(e => e.purpose)
                    .HasMaxLength(1000)
                    .HasColumnName("PURPOSE");

                entity.Property(e => e.raccno)
                    .HasMaxLength(1000)
                    .HasColumnName("RACCNO");

                entity.Property(e => e.ramt)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("RAMT");

                entity.Property(e => e.rbank)
                    .HasMaxLength(1000)
                    .HasColumnName("RBANK");

                entity.Property(e => e.rbname)
                    .HasMaxLength(1000)
                    .HasColumnName("RBNAME");

                entity.Property(e => e.rbtown)
                    .HasMaxLength(1000)
                    .HasColumnName("RBTOWN");

                entity.Property(e => e.rcbdmb)
                    .HasMaxLength(1000)
                    .HasColumnName("RCBDMB");

                entity.Property(e => e.rccountry)
                    .HasMaxLength(1000)
                    .HasColumnName("RCCOUNTRY");

                entity.Property(e => e.rccr)
                    .HasMaxLength(1000)
                    .HasColumnName("RCCR");

                entity.Property(e => e.rcname)
                    .HasMaxLength(1000)
                    .HasColumnName("RCNAME");

                entity.Property(e => e.rcstreet)
                    .HasMaxLength(1000)
                    .HasColumnName("RCSTREET");

                entity.Property(e => e.rctown)
                    .HasMaxLength(1000)
                    .HasColumnName("RCTOWN");

                entity.Property(e => e.refcd)
                    .HasMaxLength(1000)
                    .HasColumnName("REFCD");

                entity.Property(e => e.refnb)
                    .HasMaxLength(1000)
                    .HasColumnName("REFNB");

                entity.Property(e => e.requestdt)
                    .HasMaxLength(1000)
                    .HasColumnName("REQUESTDT");

                entity.Property(e => e.saccno)
                    .HasMaxLength(1000)
                    .HasColumnName("SACCNO");

                entity.Property(e => e.samt)
                    .HasColumnType("numeric(20, 4)")
                    .HasColumnName("SAMT");

                entity.Property(e => e.sbank)
                    .HasMaxLength(1000)
                    .HasColumnName("SBANK");

                entity.Property(e => e.sbbdmb)
                    .HasMaxLength(1000)
                    .HasColumnName("SBBDMB");

                entity.Property(e => e.sbcountry)
                    .HasMaxLength(1000)
                    .HasColumnName("SBCOUNTRY");

                entity.Property(e => e.sbname)
                    .HasMaxLength(1000)
                    .HasColumnName("SBNAME");

                entity.Property(e => e.sbpcode)
                    .HasMaxLength(1000)
                    .HasColumnName("SBPCODE");

                entity.Property(e => e.sbstreet)
                    .HasMaxLength(1000)
                    .HasColumnName("SBSTREET");

                entity.Property(e => e.sbtown)
                    .HasMaxLength(1000)
                    .HasColumnName("SBTOWN");

                entity.Property(e => e.sccr)
                    .HasMaxLength(1000)
                    .HasColumnName("SCCR");

                entity.Property(e => e.sdbdmb)
                    .HasMaxLength(1000)
                    .HasColumnName("SDBDMB");

                entity.Property(e => e.sdcountry)
                    .HasMaxLength(1000)
                    .HasColumnName("SDCOUNTRY");

                entity.Property(e => e.sdname)
                    .HasMaxLength(1000)
                    .HasColumnName("SDNAME");

                entity.Property(e => e.sdpcode)
                    .HasMaxLength(1000)
                    .HasColumnName("SDPCODE");

                entity.Property(e => e.sdstreet)
                    .HasMaxLength(1000)
                    .HasColumnName("SDSTREET");

                entity.Property(e => e.sdtown)
                    .HasMaxLength(1000)
                    .HasColumnName("SDTOWN");

                entity.Property(e => e.strinfor)
                    .HasMaxLength(1000)
                    .HasColumnName("STRINFOR");

                entity.Property(e => e.tranref)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TRANREF");

                entity.Property(e => e.txrefid)
                    .HasMaxLength(1000)
                    .HasColumnName("TXREFID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
        }
    }
}