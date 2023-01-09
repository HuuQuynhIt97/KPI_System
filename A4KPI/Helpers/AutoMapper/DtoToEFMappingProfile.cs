using AutoMapper;
using A4KPI.DTO;
using A4KPI.DTO.auth;
using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Helpers.AutoMapper
{
    public class DtoToEFMappingProfile : Profile
    {
        public DtoToEFMappingProfile()
        {
            CreateMap<FunctionDto, FunctionSystem>();
            CreateMap<ModuleDto, Module>();
            CreateMap<RoleDto, Role>();
            CreateMap<AccountDto, Account>()
                .ForMember(d => d.AccountType, o => o.Ignore());
            CreateMap<AccountTypeDto, AccountType>()
                .ForMember(d => d.Accounts, o => o.Ignore());
            CreateMap<AccountGroupDto, AccountGroup>();
            CreateMap<UserForDetailDto, Account>();
            CreateMap<OCDto, OC>();
            CreateMap<AccountGroupAccountDto, AccountGroupAccount>();
            CreateMap<KPINewDto, KPINew>();
            CreateMap<TargetYTDDto, TargetYTD>();
            CreateMap<TargetDto, Target>();
            CreateMap<ActionDto, Models.Action>();
            CreateMap<SettingMonthlyDto, SettingMonthly>();
            CreateMap<CampaignDto, Campaign>();

            CreateMap<AttitudeHeadingDto, AttitudeHeading>();
            CreateMap<AttitudeBehaviorDto, AttitudeBehavior>();
            CreateMap<AttitudeAttchmentDto, AttitudeAttchment>();
            CreateMap<AttitudeCategoryDto, AttitudeCategory>();
            CreateMap<AttitudeKeypointDto, AttitudeKeypoint>();
            CreateMap<AttitudeScoreDto, AttitudeScore>();
            CreateMap<JobTitleDto, JobTitle>();
            CreateMap<KeypointDto, Keypoint>();
            CreateMap<BehaviorDto, Behavior>();
            CreateMap<PointDto, Point>(); 
            CreateMap<KPIScoreDto, KPIScore>();
            CreateMap<AttitudeSubmitDto, AttitudeSubmit>();
            CreateMap<NewAttitudeContentDto, NewAttitudeContent>();
            CreateMap<NewAttitudeScoreDto, NewAttitudeScore>();
            CreateMap<NewAttitudeAttchmentDto, NewAttitudeAttchment>();
            CreateMap<NewAttitudeEvaluationDto, NewAttitudeEvaluation>();
            CreateMap<NewAttitudeSubmitDto, NewAttitudeSubmit>();
            CreateMap<AccountCampaignDto, AccountCampaign>();
            CreateMap<PerfomanceEvaluationTypeDto, PerfomanceEvaluationType>();
            CreateMap<PerfomanceEvaluationImpactDto, PerfomanceEvaluationImpact>();

            CreateMap<SpecialContributionScoreDto, SpecialContributionScore>();
            CreateMap<SpecialCompactDto, SpecialCompact>();
            CreateMap<SpecialRatioDto, SpecialRatio>();
            CreateMap<SpecialScoreDto, SpecialScore>();
            CreateMap<SpecialTypeDto, SpecialType>();
            CreateMap<KPIScoreAttchmentDto, KPIScoreAttchment>();
            CreateMap<CommitteeScoreDto, CommitteeScore>();
            CreateMap<HRCommentCmteeDto, HRCommentCmtee>();
            CreateMap<CommitteeSequenceDto, CommitteeSequence>();
        }
    }
}
