using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using A4KPI.Models;
using A4KPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace A4KPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<AttitudeBehavior> Behavior { get; set; }
        public DbSet<BehaviorCheck> BehaviorCheck { get; set; }
        public DbSet<AttitudeAttchment> AttitudeAttchment { get; set; }
        public DbSet<AttitudeCategory> AttitudeCategory { get; set; }
        public DbSet<AttitudeHeading> AttitudeHeading { get; set; }
        public DbSet<AttitudeKeypoint> AttitudeKeypoint { get; set; }
        public DbSet<AttitudeScore> AttitudeScore { get; set; }
        public DbSet<Point> Point { get; set; }
        public DbSet<Score> Score { get; set; }
        public DbSet<KPIScore> KPIScore { get; set; }
        public DbSet<AttitudeSubmit> AttitudeSubmit { get; set; }
        public DbSet<SystemFlow> SystemFlow { get; set; }
        public DbSet<UserSystemFlow> UserSystemFlows { get; set; }
        public DbSet<CommitteeScore> CommitteeScore { get; set; }
        public DbSet<HRCommentCmtee> HRCommentCmtee { get; set; }
        public DbSet<CommitteeSequence> CommitteeSequence { get; set; }
        public DbSet<NewAttitudeContent> NewAttitudeContent { get; set; }
        public DbSet<NewAttitudeScore> NewAttitudeScore { get; set; }
        public DbSet<NewAttitudeAttchment> NewAttitudeAttchment { get; set; }
        public DbSet<NewAttitudeEvaluation> NewAttitudeEvaluation { get; set; }
        public DbSet<NewAttitudeSubmit> NewAttitudeSubmit { get; set; }
        public DbSet<AccountCampaign> AccountCampaign { get; set; }
        public DbSet<PerfomanceEvaluationType> PerfomanceEvaluationType { get; set; }
        public DbSet<PerfomanceEvaluationImpact> PerfomanceEvaluationImpact { get; set; }

        public DbSet<SpecialContributionScore> SpecialContributionScore { get; set; }
        public DbSet<SpecialType> SpecialType { get; set; }
        public DbSet<SpecialScore> SpecialScore { get; set; }
        public DbSet<SpecialRatio> SpecialRatio { get; set; }
        public DbSet<SpecialCompact> SpecialCompact { get; set; }
        public DbSet<KPIScoreAttchment> KPIScoreAttchment { get; set; }

        public DbSet<Models.Permission> Permisions { get; set; }
        public DbSet<Models.Option> Options { get; set; }
        public DbSet<Models.Module> Modules { get; set; }
        public DbSet<SystemLanguage> SystemLanguages { get; set; }
        public DbSet<ModuleTranslation> ModuleTranslations { get; set; }
        public DbSet<FunctionTranslation> FunctionTranslations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Models.Version> Versions { get; set; }
        public DbSet<TargetPIC> TargetPICs { get; set; }
        public DbSet<OptionInFunctionSystem> OptionInFunctionSystems { get; set; }
        public DbSet<FunctionSystem> FunctionSystem { get; set; }
        public DbSet<Campaign> Campaign { get; set; }
        public DbSet<Evaluation> Evaluation { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<KPINew> KPINews { get; set; }
        public DbSet<KPIAccount> KPIAccounts { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<AccountGroup> AccountGroups { get; set; }
        public DbSet<AccountGroupAccount> AccountGroupAccount { get; set; }
        public DbSet<OC> OCs { get; set; }
        public DbSet<Models.Action> Actions { get; set; }
        public DbSet<Do> Do { get; set; }
        public DbSet<SettingMonthly> SettingMonthly { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<TargetYTD> TargetYTDs { get; set; }
        public DbSet<ActionStatus> ActionStatus { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<AttitudeAttchment> AttitudeAttchments { get; set; }
        public DbSet<AttitudeBehavior> AttitudeBehaviors { get; set; }
        public DbSet<Behavior> Behaviors { get; set; }
        public DbSet<AttitudeCategory> AttitudeCategories { get; set; }
        public DbSet<AttitudeHeading> AttitudeHeadings { get; set; }
        public DbSet<AttitudeKeypoint> AttitudeKeypoints { get; set; }
        public DbSet<Keypoint> Keypoints { get; set; }
        public DbSet<AttitudeScore> AttitudeScores { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {


            modelBuilder.Entity<Permission>()
            .HasKey(a => new { a.OptionID, a.FunctionSystemID, a.RoleID });

            modelBuilder.Entity<OptionInFunctionSystem>()
            .HasKey(a => new { a.OptionID, a.FunctionSystemID });
            // modelBuilder.Entity<OptionInFunctionSystem>()
            //.HasKey(a => new { a.ActionID, a.FunctionSystemID });

            //modelBuilder.Entity<OCAccount>()
            //   .HasOne(s => s.OC)
            //   .WithMany(ta => ta.OCAccounts)
            //   .HasForeignKey(u => u.OCId)
            //   .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Do>()
           .HasOne(s => s.Action)
           .WithMany(g => g.Does)
           .HasForeignKey(s => s.ActionId);

        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<EntityEntry> modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (EntityEntry item in modified)
            {
                if (item.Entity is IDateTracking changedOrAddedItem)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.CreatedTime = changedOrAddedItem.CreatedTime == DateTime.MinValue ? DateTime.Now : changedOrAddedItem.CreatedTime;
                    }
                    else
                    {
                        changedOrAddedItem.ModifiedTime = DateTime.Now;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.LogTo(Console.WriteLine);
    }
}
