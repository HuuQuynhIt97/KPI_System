using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI.Constants;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using A4KPI._Repositories.Interface;
using A4KPI._Services.Interface;

namespace A4KPI._Services.Services
{
    public class CampaignService : ICampaignService
    {
        private OperationResult operationResult;
        private readonly ICampaignRepository _repo;
        private readonly IEvaluationRepository _repoEval;
        private readonly IAccountRepository _repoAc;
        private readonly IUserRoleRepository _repoUserRole;
        private readonly IRoleRepository _repoRole;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IAttitudeScoreRepository _repoAttitudeScore;
        private readonly IAttitudeHeadingRepository _repoAttitudeHeading;
        private readonly IAttitudeCategoryRepository _repoAttitudeCategory;
        private readonly IAttitudeKeypointRepository _repoAttitudeKeypoint;
        private readonly IKeypointRepository _repoKeypoint;
        private readonly IAttitudeBehaviorRepository _repoAttitudeBehavior;
        private readonly IBehaviorRepository _repoBehavior;
        private readonly IAttitudeSubmitRepository _repoAttSubmit;
        private readonly ICommitteeScoreRepository _repoCommitteeScore;
        private readonly ICommitteeSequenceRepository _repoCommitteeSequence;
        private readonly INewAttitudeScoreRepository _repoNewAttitudeScore;
        private readonly INewAttitudeAttchmentRepository _repoNewAttitudeAttchment;
        private readonly INewAttitudeEvaluationRepository _repoNewAttitudeEvaluation;
        private readonly IAccountCampaignRepository _repoAcCampaign;
        public CampaignService(
            ICampaignRepository repo,
            IAttitudeSubmitRepository repoAttSubmit,
            IAttitudeScoreRepository repoAttitudeScore,
            IAttitudeHeadingRepository repoAttitudeHeading,
            IAttitudeCategoryRepository repoAttitudeCategory,
            IAttitudeKeypointRepository repoAttitudeKeypoint,
            IKeypointRepository repoKeypoint,
            IAttitudeBehaviorRepository repoAttitudeBehavior,
            IBehaviorRepository repoBehavior,
            IAccountRepository repoAc,
            IEvaluationRepository repoEval,
            IUserRoleRepository repoUserRole,
            IRoleRepository repoRole,
            ICommitteeScoreRepository repoCommitteeScore,
            ICommitteeSequenceRepository repoCommitteeSequence,
            INewAttitudeScoreRepository repoNewAttitudeScore,
            INewAttitudeAttchmentRepository repoNewAttitudeAttchment,
            INewAttitudeEvaluationRepository repoNewAttitudeEvaluation,
            IAccountCampaignRepository repoAcCampaign,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoAttSubmit = repoAttSubmit;
            _repoAttitudeScore = repoAttitudeScore;
            _repoAttitudeHeading = repoAttitudeHeading;
            _repoAttitudeCategory = repoAttitudeCategory;
            _repoAttitudeKeypoint = repoAttitudeKeypoint;
            _repoKeypoint = repoKeypoint;
            _repoAttitudeBehavior = repoAttitudeBehavior;
            _repoBehavior = repoBehavior;
            _repoAc = repoAc;
            _repoEval = repoEval;
            _repoUserRole = repoUserRole;
            _repoRole = repoRole;
            _repoCommitteeScore = repoCommitteeScore;
            _repoCommitteeSequence = repoCommitteeSequence;
            _repoNewAttitudeScore = repoNewAttitudeScore;
            _repoNewAttitudeAttchment = repoNewAttitudeAttchment;
            _repoNewAttitudeEvaluation = repoNewAttitudeEvaluation;
            _repoAcCampaign = repoAcCampaign;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<OperationResult> AddAsync(CampaignDto model)
        {
            var add = _mapper.Map<Campaign>(model);
            add.CreatedTime = DateTime.Now;
            _repo.Add(add);

            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = add
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }


        public async Task<List<CampaignDto>> GetAllAsync()
        {
            var result = await _repo.FindAll().Select(x => new CampaignDto {
                ID =  x.ID,
                Name =  x.Name,
                StartMonth = x.StartMonth,
                EndMonth = x.EndMonth,
                MonthName = x.MonthName,
                Year = x.Year,
                IsStart = x.IsStart,
                CreatedTime = x.CreatedTime.ToStringDateTime("yyyy-MM-dd"),
                Creator = _repoAc.FindById(x.CreatedBy).FullName
            }).OrderByDescending(x => x.ID).ToListAsync();
            return result;
        }
       
       public async Task<OperationResult> GenerateEvaluation(int campaignID)
        {
            var update = _repo.FindById(campaignID);
            update.IsStart = !update.IsStart;
            var check_user_generate =  await _repoAc.FindAll(x => x.AccountType.Code != Systems.Administrator).ToListAsync();
            var list_add = new List<Evaluation>();
            foreach (var item in check_user_generate)
            {
                var role_id = _repoUserRole.FindAll(x => x.UserID == item.Id).FirstOrDefault().RoleID;
                var role_guard = _repoRole.FindById(role_id).Code;
                if (role_guard != SystemRole.SYSTEMADMIN)
                {
                    var check = _repoEval.FindAll(x => x.CampaignID == campaignID && x.UserID == item.Id).FirstOrDefault();
                    if (check == null)
                    {
                        var item_add = new Evaluation
                        {
                            UserID = item.Id,
                            CampaignID = campaignID
                        };
                        list_add.Add(item_add);
                    }
                }
                
            }


            var attitudeHeadings = _repoAttitudeHeading.FindAll().ToList();
            var attitudeCategorys = new List<AttitudeCategory>();
            foreach (var attitudeHeading in attitudeHeadings)
            {
                var attitudeScoreDto = new AttitudeScoreDto();
                var addAttitudeScore = _mapper.Map<AttitudeScore>(attitudeScoreDto);
                addAttitudeScore.CampaignID = campaignID;
                addAttitudeScore.AttitudeHeadingID = attitudeHeading.ID;
                _repoAttitudeScore.Add(addAttitudeScore);
                await _repoAttitudeScore.SaveAll();

                var attitudeCategory_first = new AttitudeCategory
                        {
                            CampaignID = campaignID,
                            AttitudeHeadingID = attitudeHeading.ID,
                            Name = "ATTITUDE_CATEGORY_WE_EXPECT"
                        };
                attitudeCategorys.Add(attitudeCategory_first);

                var attitudeCategory_last = new AttitudeCategory
                        {
                            CampaignID = campaignID,
                            AttitudeHeadingID = attitudeHeading.ID,
                            Name = "ATTITUDE_CATEGORY_WE_DONT_EXPECT"
                        };
                attitudeCategorys.Add(attitudeCategory_last);
                

            }
            _repoAttitudeCategory.AddRange(attitudeCategorys);
            await _repoAttitudeCategory.SaveAll();
            var attitudeKeypoints = new List<AttitudeKeypoint>();
            var keypoints = _repoKeypoint.FindAll().ToList();
            
            foreach (var keypoint in keypoints)
            {
                
                var attitudeCategoryFirst = _repoAttitudeCategory.FindAll(x => x.AttitudeHeadingID == keypoint.AttitudeHeadingID && x.CampaignID == campaignID).OrderBy(x => x.ID).FirstOrDefault();
                var attitudeCategoryLast = _repoAttitudeCategory.FindAll(x => x.AttitudeHeadingID == keypoint.AttitudeHeadingID && x.CampaignID == campaignID).OrderBy(x => x.ID).LastOrDefault();
                if (keypoint.Level > 2)
                {
                    var attitudeKeypoint = new AttitudeKeypoint
                        {
                            Name = keypoint.Name,
                            Level = keypoint.Level,
                            AttitudeCategoryID = attitudeCategoryFirst.ID,
                            AttitudeHeadingID = keypoint.AttitudeHeadingID
                        };
                    attitudeKeypoints.Add(attitudeKeypoint);
                }
                else
                {
                    var attitudeKeypoint = new AttitudeKeypoint
                        {
                            Name = keypoint.Name,
                            Level = keypoint.Level,
                            AttitudeCategoryID = attitudeCategoryLast.ID,
                            AttitudeHeadingID = keypoint.AttitudeHeadingID
                        };
                    attitudeKeypoints.Add(attitudeKeypoint);
                }

                
            }
            _repoAttitudeKeypoint.AddRange(attitudeKeypoints);
            await _repoAttitudeKeypoint.SaveAll();


            var add_attitudeBehaviors = new List<AttitudeBehavior>();

            var behaviors = _repoBehavior.FindAll().ToList();
            foreach (var item_behavior in behaviors)
            {
              var list_attitudeKeypoints_created = attitudeKeypoints.FindAll(x => x.AttitudeHeadingID == item_behavior.AttitudeHeadingID).ToList();
              if (item_behavior.AttitudeHeadingID != 4)
              {
                foreach (var item_attitudeKeypoints_created in list_attitudeKeypoints_created)
                {
                  var result = add_attitudeBehaviors.FindAll(x => x.AttitudeKeypointID == item_attitudeKeypoints_created.ID).Count();
                  if (result <= 1)
                  {
                    var attitudeBehavior = new AttitudeBehavior
                          {
                              Name = item_behavior.Name,
                              AttitudeKeypointID = item_attitudeKeypoints_created.ID,
                              AttitudeHeadingID = item_behavior.AttitudeHeadingID
                          };
                      add_attitudeBehaviors.Add(attitudeBehavior);
                    break;
                  }
                  else
                  {
                    continue;
                  }
                }
              }
              else
              {
                foreach (var item_attitudeKeypoints_created in list_attitudeKeypoints_created)
                {
                  var result = add_attitudeBehaviors.FindAll(x => x.AttitudeKeypointID == item_attitudeKeypoints_created.ID).Count();
                  var result_last = list_attitudeKeypoints_created.OrderBy(x => x.Level).FirstOrDefault();
                  if (item_attitudeKeypoints_created.ID == result_last.ID && result == 2)
                  {
                    var attitudeBehavior = new AttitudeBehavior
                          {
                              Name = item_behavior.Name,
                              AttitudeKeypointID = item_attitudeKeypoints_created.ID,
                              AttitudeHeadingID = item_behavior.AttitudeHeadingID
                          };
                      add_attitudeBehaviors.Add(attitudeBehavior);
                    break;
                  }
                  if (result <= 1)
                  {
                    var attitudeBehavior = new AttitudeBehavior
                          {
                              Name = item_behavior.Name,
                              AttitudeKeypointID = item_attitudeKeypoints_created.ID,
                              AttitudeHeadingID = item_behavior.AttitudeHeadingID
                          };
                      add_attitudeBehaviors.Add(attitudeBehavior);
                    break;
                  }
                  else
                  {
                    continue;
                  }
                }
              }
              
            }
            _repoAttitudeBehavior.AddRange(add_attitudeBehaviors);
            await _repoAttitudeBehavior.SaveAll();
            
            try
            {
                 _repo.Update(update);
                _repoEval.AddRange(list_add);
                await _repoEval.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> GenerateAttitude(int campaignID)
        {
            var attitudeHeadings = _repoAttitudeHeading.FindAll().ToList();
            var attitudeCategorys = new List<AttitudeCategory>();
            foreach (var attitudeHeading in attitudeHeadings)
            {
                var attitudeScoreDto = new AttitudeScoreDto();
                var addAttitudeScore = _mapper.Map<AttitudeScore>(attitudeScoreDto);
                addAttitudeScore.CampaignID = campaignID;
                addAttitudeScore.AttitudeHeadingID = attitudeHeading.ID;
                _repoAttitudeScore.Add(addAttitudeScore);
                await _repoAttitudeScore.SaveAll();

                var attitudeCategory_first = new AttitudeCategory
                        {
                            CampaignID = campaignID,
                            AttitudeHeadingID = attitudeHeading.ID,
                            Name = "We expect team members to"
                        };
                attitudeCategorys.Add(attitudeCategory_first);

                var attitudeCategory_last = new AttitudeCategory
                        {
                            CampaignID = campaignID,
                            AttitudeHeadingID = attitudeHeading.ID,
                            Name = "We don’t expect team members to"
                        };
                attitudeCategorys.Add(attitudeCategory_last);
                

            _repoAttitudeCategory.AddRange(attitudeCategorys);
            }
            await _repoAttitudeCategory.SaveAll();

            var keypoints = _repoKeypoint.FindAll().ToList();
            var attitudeKeypoints = new List<AttitudeKeypoint>();
            foreach (var keypoint in keypoints)
            {
                
                var attitudeCategoryFirst = _repoAttitudeCategory.FindAll(x => x.AttitudeHeadingID == keypoint.AttitudeHeadingID && x.CampaignID == campaignID).OrderBy(x => x.ID).FirstOrDefault();
                var attitudeCategoryLast = _repoAttitudeCategory.FindAll(x => x.AttitudeHeadingID == keypoint.AttitudeHeadingID && x.CampaignID == campaignID).OrderBy(x => x.ID).LastOrDefault();
                if (keypoint.Level > 2)
                {
                    var attitudeKeypoint = new AttitudeKeypoint
                        {
                            Name = keypoint.Name,
                            Level = keypoint.Level,
                            AttitudeCategoryID = attitudeCategoryFirst.ID,
                            AttitudeHeadingID = keypoint.AttitudeHeadingID
                        };
                    attitudeKeypoints.Add(attitudeKeypoint);
                }
                else
                {
                    var attitudeKeypoint = new AttitudeKeypoint
                        {
                            Name = keypoint.Name,
                            Level = keypoint.Level,
                            AttitudeCategoryID = attitudeCategoryLast.ID,
                            AttitudeHeadingID = keypoint.AttitudeHeadingID
                        };
                    attitudeKeypoints.Add(attitudeKeypoint);
                }

                
            }
            _repoAttitudeKeypoint.AddRange(attitudeKeypoints);
            await _repoAttitudeKeypoint.SaveAll();

            var add_attitudeBehaviors = new List<AttitudeBehavior>();
            // foreach (var attitudeHeading in attitudeHeadings)
            // {
            //   var list_attitudeKeypoints_created = attitudeKeypoints.FindAll(x => x.AttitudeHeadingID == attitudeHeading.ID).ToList();
            //   foreach (var item_attitudeKeypoint in attitudeKeypoints)
            //   {
            //     var behaviorsss = _repoBehavior.FindAll(x => x.AttitudeHeadingID == item_attitudeKeypoint.AttitudeHeadingID).ToList();

            //   }
            // }


            var behaviors = _repoBehavior.FindAll().ToList();
            foreach (var item_behavior in behaviors)
            {
              var list_attitudeKeypoints_created = attitudeKeypoints.FindAll(x => x.AttitudeHeadingID == item_behavior.AttitudeHeadingID).ToList();
              if (item_behavior.AttitudeHeadingID != 4)
              {
                foreach (var item_attitudeKeypoints_created in list_attitudeKeypoints_created)
                {
                  var result = add_attitudeBehaviors.FindAll(x => x.AttitudeKeypointID == item_attitudeKeypoints_created.ID).Count();
                  if (result <= 1)
                  {
                    var attitudeBehavior = new AttitudeBehavior
                          {
                              Name = item_behavior.Name,
                              AttitudeKeypointID = item_attitudeKeypoints_created.ID,
                              AttitudeHeadingID = item_behavior.AttitudeHeadingID
                          };
                      add_attitudeBehaviors.Add(attitudeBehavior);
                    break;
                  }
                  else
                  {
                    continue;
                  }
                }
              }
              else
              {
                foreach (var item_attitudeKeypoints_created in list_attitudeKeypoints_created)
                {
                  var result = add_attitudeBehaviors.FindAll(x => x.AttitudeKeypointID == item_attitudeKeypoints_created.ID).Count();
                  var result_last = list_attitudeKeypoints_created.OrderBy(x => x.Level).FirstOrDefault();
                  if (item_attitudeKeypoints_created.ID == result_last.ID && result == 2)
                  {
                    var attitudeBehavior = new AttitudeBehavior
                          {
                              Name = item_behavior.Name,
                              AttitudeKeypointID = item_attitudeKeypoints_created.ID,
                              AttitudeHeadingID = item_behavior.AttitudeHeadingID
                          };
                      add_attitudeBehaviors.Add(attitudeBehavior);
                    break;
                  }
                  if (result <= 1)
                  {
                    var attitudeBehavior = new AttitudeBehavior
                          {
                              Name = item_behavior.Name,
                              AttitudeKeypointID = item_attitudeKeypoints_created.ID,
                              AttitudeHeadingID = item_behavior.AttitudeHeadingID
                          };
                      add_attitudeBehaviors.Add(attitudeBehavior);
                    break;
                  }
                  else
                  {
                    continue;
                  }
                }
              }
              
            }
            // _repoAttitudeBehavior.AddRange(add_attitudeBehaviors);
            // await _repoAttitudeBehavior.SaveAll();
            
            
            try
            {
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
       
        public async Task<OperationResult> UpdateAsync(CampaignDto model)
        {
            var update = _mapper.Map<Campaign>(model);
            _repo.Update(update);

            try
            {
                await _repo.SaveAll();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = update
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var query = _repo.FindById(id);
            var list_eval = _repoEval.FindAll(x => x.CampaignID == id).ToList();
            var list_attSubmit = _repoAttSubmit.FindAll(x => x.CampaignID == id).ToList();
            var attitudeScores = _repoAttitudeScore.FindAll(x => x.CampaignID == id).ToList();
            var attitudeCategorys = _repoAttitudeCategory.FindAll(x => x.CampaignID == id).ToList();
            var committeeScores = _repoCommitteeScore.FindAll(x => x.CampaignID == id).ToList();
            var committeeSequences = _repoCommitteeSequence.FindAll(x => x.CampaignID == id).ToList();
            var newAttitudeScores = _repoNewAttitudeScore.FindAll(x => x.CampaignID == id).ToList();
            var newAttitudeAttchments = _repoNewAttitudeAttchment.FindAll(x => x.CampaignID == id).ToList();
            var newAttitudeEvaluations = _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == id).ToList();
            var accountCampaigns = _repoAcCampaign.FindAll(x => x.CampaignID == id).ToList();
        
            foreach (var element in attitudeCategorys)
            {
                var attitudeKeypoints = _repoAttitudeKeypoint.FindAll(x => x.AttitudeCategoryID == element.ID).ToList();
                foreach (var item in attitudeKeypoints)
                {
                    var attitudeBehaviors = _repoAttitudeBehavior.FindAll(x => x.AttitudeKeypointID == item.ID).ToList();
                    _repoAttitudeBehavior.RemoveMultiple(attitudeBehaviors);
                }
                _repoAttitudeKeypoint.RemoveMultiple(attitudeKeypoints);
            }
            _repoAttitudeCategory.RemoveMultiple(attitudeCategorys);
            _repoAttSubmit.RemoveMultiple(list_attSubmit);
            _repoAttitudeScore.RemoveMultiple(attitudeScores);
            _repo.Remove(query);
            _repoEval.RemoveMultiple(list_eval);
            _repoCommitteeScore.RemoveMultiple(committeeScores);
            _repoCommitteeSequence.RemoveMultiple(committeeSequences);
            _repoNewAttitudeScore.RemoveMultiple(newAttitudeScores);
            _repoNewAttitudeAttchment.RemoveMultiple(newAttitudeAttchments);
            _repoNewAttitudeEvaluation.RemoveMultiple(newAttitudeEvaluations);
            _repoNewAttitudeEvaluation.RemoveMultiple(newAttitudeEvaluations);
            _repoAcCampaign.RemoveMultiple(accountCampaigns);

            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = query
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        
    }

}
