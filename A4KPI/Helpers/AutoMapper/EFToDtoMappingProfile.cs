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
    public class EFToDtoMappingProfile : Profile
    {
        public EFToDtoMappingProfile()
        {
            CreateMap<FunctionSystem, FunctionDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<Module, ModuleDto>();
            CreateMap<Account, AccountDto>()
                .ForMember(d => d.AccountGroupText, o => o.MapFrom(s => s.AccountGroupAccount.Count > 0 ? String.Join(",", s.AccountGroupAccount.Select(x=>x.AccountGroup.Name)) : ""))
                .ForMember(d => d.AccountGroupIds, o => o.MapFrom(s => s.AccountGroupAccount.Select(x=>x.AccountGroup.Id) ))
                ;
            CreateMap<AccountType, AccountTypeDto>();
            CreateMap<AccountGroup, AccountGroupDto>();
           
            CreateMap<Account, UserForDetailDto>()
                .ForMember(d => d.IsLeader, o => o.MapFrom(s => s.Leader.HasValue && s.Leader != 0))
                .ForMember(d => d.AccountGroupPositions, o => o.MapFrom(s => s.AccountGroupAccount.Select(x=>x.AccountGroup.Position)))
                .ForMember(d => d.IsManager, o => o.MapFrom(s => s.Manager.HasValue && s.Manager != 0))
                ;
            CreateMap<OC, OCDto>();
            CreateMap<AccountGroupAccount, AccountGroupAccountDto>();
           
            CreateMap<KPINew, KPINewDto>();
            CreateMap<TargetYTD, TargetYTDDto>();
            CreateMap<Target, TargetDto>();
            CreateMap<Models.Action, ActionDto>();
            CreateMap<SettingMonthly, SettingMonthlyDto>();
            CreateMap<Campaign, CampaignDto>()
                .ForMember(d => d.CreatedTime, o => o.MapFrom(s => s.CreatedTime.ToStringDateTime("yyyy-MM-dd")))
                ;
            CreateMap<AttitudeHeading, AttitudeHeadingDto>();
            CreateMap<AttitudeBehavior, AttitudeBehaviorDto>();
            CreateMap<AttitudeAttchment, AttitudeAttchmentDto>();
            CreateMap<AttitudeCategory, AttitudeCategoryDto>();
            CreateMap<AttitudeKeypoint, AttitudeKeypointDto>();
            CreateMap<AttitudeScore, AttitudeScoreDto>();

            CreateMap<JobTitle, JobTitleDto>();
            CreateMap<Keypoint, KeypointDto>();
            CreateMap<Behavior, BehaviorDto>();
            CreateMap<Point, PointDto>();
            CreateMap<KPIScore, KPIScoreDto>();
            CreateMap<AttitudeSubmit, AttitudeSubmitDto>();
            CreateMap<NewAttitudeContent, NewAttitudeContentDto>();
            CreateMap<NewAttitudeScore, NewAttitudeScoreDto>();
            CreateMap<NewAttitudeAttchment, NewAttitudeAttchmentDto>();
            CreateMap<NewAttitudeEvaluation, NewAttitudeEvaluationDto>();
            CreateMap<NewAttitudeSubmit, NewAttitudeSubmitDto>();
            CreateMap<AccountCampaign, AccountCampaignDto>();
            CreateMap<PerfomanceEvaluationType, PerfomanceEvaluationTypeDto>();
            CreateMap<PerfomanceEvaluationImpact, PerfomanceEvaluationImpactDto>();


            CreateMap<SpecialContributionScore, SpecialContributionScoreDto>();
            CreateMap<SpecialCompact, SpecialCompactDto>();
            CreateMap<SpecialRatio, SpecialRatioDto>();
            CreateMap<SpecialScore, SpecialScoreDto>();
            CreateMap<SpecialType, SpecialTypeDto>();
            CreateMap<KPIScoreAttchment, KPIScoreAttchmentDto>();
            CreateMap<CommitteeScore, CommitteeScoreDto>();
            CreateMap<HRCommentCmtee, HRCommentCmteeDto>();
            CreateMap<CommitteeSequence, CommitteeSequenceDto>();
        }
    }
}
