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

    public class NewAttitudeScoreService : INewAttitudeScoreService
    {
        private readonly INewAttitudeContentRepository _repoNewAttContent;
        private readonly INewAttitudeScoreRepository _repoNewAttScore;
        private readonly INewAttitudeAttchmentRepository _repoNewAttAttchment;
        private readonly INewAttitudeEvaluationRepository _repoNewAttitudeEvaluation;
        private readonly INewAttitudeSubmitRepository _repoNewAttitudeSubmit;

        private readonly IAttitudeSubmitRepository _repoAttitudeSubmit;
        private readonly IEvaluationRepository _repoEvaluation;
        private readonly IAccountRepository _repoAccount;
        private readonly IAccountCampaignRepository _repoAccountCampaign;
        private readonly ICampaignRepository _repoCampaign;
        private readonly ISystemFlowRepository _repoSystemFlow;
        private readonly IOCRepository _repoOC;

        private readonly IMapper _mapper;
        private readonly IMailExtension _mailHelper;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;
        private OperationResult operationResult;

        public NewAttitudeScoreService(
            INewAttitudeContentRepository repoNewAttContent,
            INewAttitudeScoreRepository repoNewAttScore,
            INewAttitudeAttchmentRepository repoNewAttAttchment,
            INewAttitudeEvaluationRepository repoNewAttitudeEvaluation,
            INewAttitudeSubmitRepository repoNewAttitudeSubmit,

            IAttitudeSubmitRepository repoAttitudeSubmit,
            IEvaluationRepository repoEvaluation,
            IAccountRepository repoAccount,
            IAccountCampaignRepository repoAccountCampaign,
            ICampaignRepository repoCampaign,
            ISystemFlowRepository repoSystemFlow,
            IOCRepository repoOC,
            
            IMapper mapper,
            IMailExtension mailExtension,
            IConfiguration configuration,
            MapperConfiguration configMapper
            )
        {
            _repoNewAttContent = repoNewAttContent;
            _repoNewAttScore = repoNewAttScore;
            _repoNewAttAttchment = repoNewAttAttchment;
            _repoNewAttitudeEvaluation = repoNewAttitudeEvaluation;
            _repoNewAttitudeSubmit = repoNewAttitudeSubmit;

            _repoAttitudeSubmit = repoAttitudeSubmit;
            _repoEvaluation = repoEvaluation;
            _repoAccount = repoAccount;
            _repoAccountCampaign = repoAccountCampaign;
            _repoCampaign = repoCampaign;
            _repoSystemFlow = repoSystemFlow;
            _repoOC = repoOC;
            
            _mapper = mapper;
            _mailHelper = mailExtension;
            _configuration = configuration;
            _configMapper = configMapper;
        }
        public async Task<List<NewAttitudeContentDto>> GetAllAsync(int campaignID, int scoreTo, int scoreFrom)
        {
            // var query = from a in await _repoNewAttScore.FindAll().ToListAsync()
            //             group a by new {
            //                 a.OrderNumber
            //             } into child
            //             join b in await _repoNewAttContent.FindAll().ToListAsync() on child.FirstOrDefault().AttitudeContenID equals b.ID
            //             select new NewAttitudeContentDto() {
            //                 OrderNumber = b.OrderNumber,
            //                 Name = b.Name,
            //                 Definition = b.Definition,
            //                 NewAttitudeScore = child.Select(x => new {
            //                            ID = "0"
            //                 }).ToList()
            //             };

            var ListNewAttitudeScore = (from a in await _repoNewAttScore.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == scoreFrom).ToListAsync()
                        join b in await _repoNewAttContent.FindAll().ToListAsync() on a.AttitudeContenID equals b.ID
                        select new NewAttitudeScoreDto() {
                                    ID = a.ID,
                                    Point1 = a.Point1,
                                    Point2 = a.Point2,
                                    Point3 = a.Point3,
                                    Point4 = a.Point4,
                                    Point5 = a.Point5,
                                    Point6 = a.Point6,
                                    Point7 = a.Point7,
                                    Point8 = a.Point8,
                                    Point9 = a.Point9,
                                    Point10 = a.Point10,
                                    CampaignID = campaignID,
                                    ScoreTo = scoreTo,
                                    ScoreFrom = scoreFrom,
                                    CreatedTime = a.CreatedTime,
                                    AttitudeContenID = a.AttitudeContenID,
                                    OrderNumber = a.OrderNumber,
                                    Name = b.Name,
                                    Definition = b.Definition,
                                    Behavior = b.Behavior
                        }).ToList();

            var query = (from a in ListNewAttitudeScore
                        group a by new {
                            a.OrderNumber,
                            a.Name,
                            a.Definition,
                        } into child
                        select new NewAttitudeContentDto() {
                            OrderNumber = child.Key.OrderNumber,
                            Name = child.Key.Name,
                            Definition = child.Key.Definition,
                            NewAttitudeAttchmentID = _repoNewAttAttchment.FindAll(x => x.ScoreFrom == scoreFrom && x.ScoreTo == scoreTo && x.OrderNumber == child.Key.OrderNumber).FirstOrDefault()
                                                    != null ? _repoNewAttAttchment.FindAll(x => x.ScoreFrom == scoreFrom && x.ScoreTo == scoreTo && x.OrderNumber == child.Key.OrderNumber).FirstOrDefault().ID : 0,
                            Comment = _repoNewAttAttchment.FindAll(x => x.CampaignID == campaignID && x.ScoreFrom == scoreFrom && x.ScoreTo == scoreTo && x.OrderNumber == child.Key.OrderNumber).FirstOrDefault()
                                                    != null ? _repoNewAttAttchment.FindAll(x => x.CampaignID == campaignID && x.ScoreFrom == scoreFrom && x.ScoreTo == scoreTo && x.OrderNumber == child.Key.OrderNumber).FirstOrDefault().Comment : "",
                            NewAttitudeScore = child.Select(y => new NewAttitudeScoreDto() {
                                                ID = y.ID,
                                                Point1 = y.Point1,
                                                Point2 = y.Point2,
                                                Point3 = y.Point3,
                                                Point4 = y.Point4,
                                                Point5 = y.Point5,
                                                Point6 = y.Point6,
                                                Point7 = y.Point7,
                                                Point8 = y.Point8,
                                                Point9 = y.Point9,
                                                Point10 = y.Point10,
                                                CampaignID = y.CampaignID,
                                                ScoreTo = y.ScoreTo,
                                                ScoreFrom = y.ScoreFrom,
                                                CreatedTime = y.CreatedTime,
                                                AttitudeContenID = y.AttitudeContenID,
                                                Behavior = y.Behavior
                                            }).ToList()
                        }).ToList();

            
            // var result = ListNewAttitudeScore.GroupBy(x => new { 
            //         x.OrderNumber,
            //         x.Name,
            //         x.Definition,
            // })
            //   .Select(x => new NewAttitudeContentDto()
            //   {
            //       OrderNumber = x.First().OrderNumber,
            //       Name = x.First().Name,
            //       Definition = x.First().Definition,
                  
            //       NewAttitudeScore = x.Select(y => new NewAttitudeScoreDto() {
            //                         ID = y.ID,
            //                         Point0_5 = y.Point0_5,
            //                         Point1 = y.Point1,
            //                         Point1_5 = y.Point1_5,
            //                         Point2 = y.Point2,
            //                         Point2_5 = y.Point2_5,
            //                         Point3 = y.Point3,
            //                         Point3_5 = y.Point3_5,
            //                         Point4 = y.Point4,
            //                         Point4_5 = y.Point4_5,
            //                         Point5 = y.Point5,
            //                         CampaignID = 0,
            //                         ScoreTo = 0,
            //                         ScoreFrom = 0,
            //                         CreatedTime = y.CreatedTime,
            //                         AttitudeContenID = y.AttitudeContenID,
            //                         Behavior = y.Behavior
            //       }).ToList(),
                  
            //   });
            
            return query;
            
        }

        public async Task<OperationResult> UpdatePointAsync(int id, string point)
        {
            var model = _repoNewAttScore.FindById(id);
            model.Point1 = false;
            model.Point2 = false;
            model.Point3 = false;
            model.Point4 = false;
            model.Point5 = false;
            model.Point6 = false;
            model.Point7 = false;
            model.Point8 = false;
            model.Point9 = false;
            model.Point10 = false;

            switch (point)
            {
                case "1":
                    model.Point1 = true;
                    break;
                case "2":
                    model.Point2 = true;
                    break;
                case "3":
                    model.Point3 = true;
                    break;
                case "4":
                    model.Point4 = true;
                    break;
                case "5":
                    model.Point5 = true;
                    break;
                case "6":
                    model.Point6 = true;
                    break;
                case "7":
                    model.Point7 = true;
                    break;
                case "8":
                    model.Point8 = true;
                    break;
                case "9":
                    model.Point9 = true;
                    break;
                case "10":
                    model.Point10 = true;
                    break;
                default:
                    break;
            }
            
            
            var update = _mapper.Map<NewAttitudeScore>(model);
            _repoNewAttScore.Update(update);

            try
            {
                await _repoNewAttScore.SaveAll();
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

        public async Task<OperationResult> UpdateCommentAsync(NewAttitudeAttchmentDto model)
        {
            // var update = _mapper.Map<NewAttitudeAttchment>(model);
            if (model.ID == 0)
            {
                var add = _mapper.Map<NewAttitudeAttchment>(model);
                add.CreatedTime = DateTime.Now;
                _repoNewAttAttchment.Add(add);
            }
            else {
                var item = _repoNewAttAttchment.FindById(model.ID);
                var update = _mapper.Map<NewAttitudeAttchment>(item);
                update.Comment = model.Comment;
                _repoNewAttAttchment.Update(update);
            }

            try
            {
                await _repoNewAttAttchment.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
            
        }

        public async Task<NewAttitudeEvaluationDto> GetAttEvaluationAsync(int campaignID, int scoreTo, int scoreFrom)
        {
            return await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == scoreFrom).ProjectTo<NewAttitudeEvaluationDto>(_configMapper).FirstOrDefaultAsync();
        }

        public async Task<OperationResult> UpdateAttEvaluationAsync(NewAttitudeEvaluationDto model)
        {
            if (model.ID == 0)
            {
                var add = _mapper.Map<NewAttitudeEvaluation>(model);
                add.CreatedTime = DateTime.Now;
                _repoNewAttitudeEvaluation.Add(add);
            }
            else {
                var item = _repoNewAttitudeEvaluation.FindById(model.ID);
                var update = _mapper.Map<NewAttitudeEvaluation>(item);
                update.FirstQuestion1 = model.FirstQuestion1;
                update.FirstQuestion2 = model.FirstQuestion2;
                update.FirstQuestion3 = model.FirstQuestion3;
                update.FirstQuestion4 = model.FirstQuestion4;
                update.FirstQuestion5 = model.FirstQuestion5;
                update.FirstQuestion6 = model.FirstQuestion6;

                update.SecondQuestion1 = model.SecondQuestion1;
                update.SecondQuestion2 = model.SecondQuestion2;
                update.SecondQuestion3 = model.SecondQuestion3;
                update.SecondQuestion4 = model.SecondQuestion4;
                update.SecondQuestion5 = model.SecondQuestion5;
                update.SecondQuestion6 = model.SecondQuestion6;

                update.ThirdQuestion = model.ThirdQuestion.Trim();
                update.FourthQuestion = model.FourthQuestion.Trim();
                _repoNewAttitudeEvaluation.Update(update);
            }

            try
            {
                await _repoNewAttitudeEvaluation.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
            
        }

        public async Task<AttitudeSubmitDto> GetNewAttitudeSubmit(int campaignID, int scoreTo)
        {
            return await _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == scoreTo).ProjectTo<AttitudeSubmitDto>(_configMapper).FirstOrDefaultAsync();
        }

        public async Task<OperationResult> CheckSubmitNewAtt(int campaignID, int scoreTo, int scoreFrom, string type)
        {
            var listAttPoint = new List<NewAttitudePointDto>();
            var listAttScore = _repoNewAttScore.FindAll(x => x.CampaignID == campaignID && x.ScoreFrom == scoreFrom && x.ScoreTo == scoreTo).ToList();
            foreach (var itemAttScore in listAttScore)
            {
                var att_Point = new NewAttitudePointDto();
                att_Point.CampaignID = itemAttScore.CampaignID;
                att_Point.ScoreFrom = itemAttScore.ScoreFrom;
                att_Point.ScoreTo = itemAttScore.ScoreTo;
                att_Point.OrderNumber = itemAttScore.OrderNumber;

                att_Point.Point = itemAttScore switch
                {
                    { Point1: true } => 1,
                    { Point2: true } => 2,
                    { Point3: true } => 3,
                    { Point4: true } => 4,
                    { Point5: true } => 5,
                    { Point6: true } => 6,
                    { Point7: true } => 7,
                    { Point8: true } => 8,
                    { Point9: true } => 9,
                    { Point10: true } => 10,
                    _ => 0
                };
                

                listAttPoint.Add(att_Point);
                
                if (!itemAttScore.Point1 && !itemAttScore.Point2 &&
                    !itemAttScore.Point3 && !itemAttScore.Point4 &&
                    !itemAttScore.Point5 && !itemAttScore.Point6 &&
                    !itemAttScore.Point7 && !itemAttScore.Point8 &&
                    !itemAttScore.Point9 && !itemAttScore.Point10)
                {
                    // return new {    
                    //     status = false,
                    //     message = "TICK_POINT_NEW_ATT"
                    // };

                    return operationResult = new OperationResult
                                        {
                                            StatusCode = HttpStatusCode.OK,
                                            Message = "TICK_POINT_NEW_ATT",
                                            Success = false
                                        };
                }
            }

            /// type L0
            if (type == "L0") 
                {
                    var listAttPointGroup = listAttPoint.GroupBy(x => x.OrderNumber).Select(x => new NewAttitudePointDto() 
                    {
                        OrderNumber = x.First().OrderNumber,
                        CampaignID = x.First().CampaignID,
                        ScoreFrom = x.First().ScoreFrom,
                        ScoreTo = x.First().ScoreTo,
                        Point = x.Sum(x => x.Point),
                    }).ToList();
                    foreach (var itemAttPoint in listAttPointGroup)
                    {
                        if (itemAttPoint.Point >= 18)
                        {
                            var queryComment = await _repoNewAttAttchment.FindAll(x => x.CampaignID == campaignID && x.ScoreFrom == scoreFrom && x.ScoreTo == scoreTo && x.OrderNumber == itemAttPoint.OrderNumber).FirstOrDefaultAsync();
                            if (queryComment == null)
                            {
                                // return new {    
                                //     status = false,
                                //     message = "MISS_COMMENT_NEW_ATT"
                                // };

                                return operationResult = new OperationResult
                                        {
                                            StatusCode = HttpStatusCode.OK,
                                            Message = "MISS_COMMENT_NEW_ATT",
                                            Success = false
                                        };
                            }
                            else if (queryComment.Comment.IsNullOrEmpty())
                            {
                                // return new {    
                                //     status = false,
                                //     message = "MISS_COMMENT_NEW_ATT"
                                // };

                                return operationResult = new OperationResult
                                        {
                                            StatusCode = HttpStatusCode.OK,
                                            Message = "MISS_COMMENT_NEW_ATT",
                                            Success = false
                                        };
                            }
                        }
                    }
            
                }
            
            var queryAttEvalution = await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == campaignID && x.ScoreFrom == scoreFrom && x.ScoreTo == scoreTo).FirstOrDefaultAsync();
            if (queryAttEvalution == null && (type == "L0" || type == "L1"))
            {
                // return new {    
                //     status = false,
                //     message = "MISS_SAVE_QUESTIONS_NEW_ATT"
                // };

                return operationResult = new OperationResult
                                        {
                                            StatusCode = HttpStatusCode.OK,
                                            Message = "MISS_SAVE_QUESTIONS_NEW_ATT",
                                            Success = false
                                        };
            }
            else
            {
                //for L0, L1
                if (type == "L0" || type == "L1")
                {
                    if ((!queryAttEvalution.FirstQuestion1 && !queryAttEvalution.FirstQuestion2 && !queryAttEvalution.FirstQuestion3
                      && !queryAttEvalution.FirstQuestion4 && !queryAttEvalution.FirstQuestion5 && !queryAttEvalution.FirstQuestion6) ||
                        (!queryAttEvalution.SecondQuestion1 && !queryAttEvalution.SecondQuestion2 && !queryAttEvalution.SecondQuestion3
                      && !queryAttEvalution.SecondQuestion4 && !queryAttEvalution.SecondQuestion5 && !queryAttEvalution.SecondQuestion6))
                    {
                        // return new {    
                        //     status = false,
                        //     message = "MISS_TICK_QUESTIONS_NEW_ATT"
                        // };

                        return operationResult = new OperationResult
                                        {
                                            StatusCode = HttpStatusCode.OK,
                                            Message = "MISS_TICK_QUESTIONS_NEW_ATT",
                                            Success = false
                                        };
                    }
                }

                //for L0
                if (type == "L0")
                {
                    if (queryAttEvalution.ThirdQuestion.IsNullOrEmpty() || queryAttEvalution.FourthQuestion.IsNullOrEmpty())
                    {
                        // return new {    
                        //         status = false,
                        //         message = "MISS_ANSWER_QUESTIONS_NEW_ATT"
                        //     };
                        return operationResult = new OperationResult
                                        {
                                            StatusCode = HttpStatusCode.OK,
                                            Message = "MISS_ANSWER_QUESTIONS_NEW_ATT",
                                            Success = false
                                        };
                    }
                }
                
                //for L1, L2, FL
                if (type == "L1")
                {
                    if (queryAttEvalution.ThirdQuestion.IsNullOrEmpty())
                    {
                        // return new {    
                        //         status = false,
                        //         message = "MISS_ANSWER_QUESTIONS_NEW_ATT"
                        //     };

                        return operationResult = new OperationResult
                                        {
                                            StatusCode = HttpStatusCode.OK,
                                            Message = "MISS_ANSWER_QUESTIONS_NEW_ATT",
                                            Success = false
                                        };
                    }
                }
            }

            // var queryAttSubmit = await _repoNewAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == scoreTo).FirstOrDefaultAsync();
            // var updateAttSubmit = _mapper.Map<NewAttitudeSubmit>(queryAttSubmit);
            // switch (type)
            // {
            //     case "L0":
            //         updateAttSubmit.IsSubmitAttitudeL0 = true;
            //         break;
            //     case "L1":
            //         updateAttSubmit.IsSubmitAttitudeL1 = true;
            //         break;
            //     case "L2":
            //         updateAttSubmit.IsSubmitAttitudeL2 = true;
            //         break;
            //     case "FL":
            //         updateAttSubmit.IsSubmitAttitudeFL = true;
            //         break;
            //     default:
            //         break;
            // }

            // _repoNewAttitudeSubmit.Update(updateAttSubmit);
            // await _repoNewAttitudeSubmit.SaveAll();

            var att_submit = _repoAttitudeSubmit.FindAll(x => x.SubmitTo == scoreTo && x.CampaignID == campaignID).FirstOrDefault();
            var systemFlow_user = _repoAccount.FindAll(x => x.Id == scoreTo).FirstOrDefault();
            switch (type)
            {
                case "L0":
                    if (systemFlow_user.SystemFlow == 1 || systemFlow_user.SystemFlow == 4)
                    {
                        // att_submit.L0Attitude = true;
                        // att_submit.BtnL0 = false;
                        // att_submit.BtnL2 = true;

                        att_submit.IsSubmitAttitudeL0 = true;
                        att_submit.BtnNewAttL0 = false;
                        att_submit.BtnNewAttL2 = true;
                    }
                    else if(systemFlow_user.SystemFlow == 2 || systemFlow_user.SystemFlow == 5)
                    {
                        // att_submit.L0Attitude = true;
                        // att_submit.BtnL0 = false;
                        // att_submit.BtnL1 = true;

                        att_submit.IsSubmitAttitudeL0 = true;
                        att_submit.BtnNewAttL0 = false;
                        att_submit.BtnNewAttL1 = true;
                    }
                    else
                    {
                        // att_submit.L0Attitude = true;
                        // att_submit.BtnL0 = false;
                        // att_submit.BtnL1 = true;

                        att_submit.IsSubmitAttitudeL0 = true;
                        att_submit.BtnNewAttL0 = false;
                        att_submit.BtnNewAttL1 = true;
                    }
                    break;
                case "L1":

                    if (systemFlow_user.SystemFlow == 2 || systemFlow_user.SystemFlow == 5)
                    {
                        // att_submit.L1Attitude = true;
                        // att_submit.BtnL1 = false;
                        // att_submit.BtnL2 = true;

                        att_submit.IsSubmitAttitudeL1 = true;
                        att_submit.BtnNewAttL1 = false;
                        att_submit.BtnNewAttL2 = true;
                    }
                    else if (systemFlow_user.SystemFlow == 3 || systemFlow_user.SystemFlow == 6)
                    {
                        // att_submit.L1Attitude = true;
                        // att_submit.BtnL1 = false;
                        // att_submit.BtnL2 = true;

                        att_submit.IsSubmitAttitudeL1 = true;
                        att_submit.BtnNewAttL1 = false;
                        att_submit.BtnNewAttL2 = true;
                    }

                    break;
                case "L2":
                    // //if (systemFlow_user.SystemFlow == 2 || systemFlow_user.SystemFlow == 5)
                    // //{
                    // //    att_submit.L2Attitude = true;
                    // //    att_submit.BtnL1 = false;
                    // //    att_submit.BtnL2 = true;
                    // //}
                    // //else if (systemFlow_user.SystemFlow == 1 || systemFlow_user.SystemFlow == 4)
                    // //{
                    // //    att_submit.L1Attitude = true;
                    // //    att_submit.BtnL1 = false;
                    // //    att_submit.BtnL2 = true;
                    // //}

                    // att_submit.L2Attitude = true;
                    // att_submit.BtnL2 = false;
                    // //att_submit.BtnL2KPI = false;

                    att_submit.IsSubmitAttitudeL2 = true;
                    att_submit.BtnNewAttL2 = false;
                    break;
                case "FL":
                    // att_submit.FLAttitude = true;
                    // att_submit.BtnFL = false;
                    // att_submit.BtnFLKPI = false;
                    // att_submit.BtnL0 = true;
                    // att_submit.BtnL0KPI = true;

                    att_submit.IsSubmitAttitudeFL = true;
                    att_submit.BtnNewAttFL = false;
                    att_submit.BtnFLKPI = false;
                    att_submit.BtnNewAttL0 = true;
                    att_submit.BtnL0KPI = true;
                    break;
            }
            var infor_user = _repoAccount.FindById(scoreTo);
            var result_message = new SignarlLoadUseDto()
            {
                L0 = infor_user.Id,
                L1 = infor_user.Manager.Value,
                L2 = infor_user.L2.Value,
                FL = infor_user.FunctionalLeader.Value
            };

            await _repoAttitudeSubmit.SaveAll();
            
            // return new {    
            //         status = true,
            //         message = "SUBMIT_PDCA_SUCCESS",
            //         SignarlData = result_message
            //     };

            return operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "SUBMIT_PDCA_SUCCESS",
                    Success = true,
                    SignarlData = result_message
                };

        }
        

        public async Task<OperationResult> GenerateNewAttitudeScore(int campaignID, int scoreTo, int scoreFrom)
        {
            var listAttScore = _repoNewAttScore.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == scoreFrom).ToList();

            if(listAttScore.Count() == 0) {
                var listAttContent = await _repoNewAttContent.FindAll().ToListAsync();
                foreach (var item in listAttContent)
                {
                    var itemAttScore = new NewAttitudeScoreDto();
                    itemAttScore.CampaignID = campaignID;
                    itemAttScore.ScoreTo = scoreTo;
                    itemAttScore.ScoreFrom = scoreFrom;
                    itemAttScore.AttitudeContenID = item.ID;
                    itemAttScore.OrderNumber = item.OrderNumber;
                    itemAttScore.CreatedTime = DateTime.Now;

                    var add = _mapper.Map<NewAttitudeScore>(itemAttScore);
                    _repoNewAttScore.Add(add);
                    await _repoNewAttScore.SaveAll();
                }
            }

            try
            {
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = listAttScore
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
            
        }

        //get page load btn score

        //for L0
        public async Task<List<EvaluationDto>> GetSelfAppraisal(int userID)
        {
            var list_evaluation = await _repoEvaluation.FindAll(x => x.UserID == userID).ToListAsync();
            var list_ac = await _repoAccount.FindAll(x => x.L0.Value).ToListAsync();

            var list_systemFlow = await _repoSystemFlow.FindAll().ToListAsync();
            var list_attSubmit = await _repoAttitudeSubmit.FindAll().ToListAsync();
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id
                          join z in list_systemFlow on y.SystemFlow equals z.SystemFlowID
                          join t in list_attSubmit on x.CampaignID equals t.CampaignID into xt
                          join c in list_campaign on x.CampaignID equals c.ID
                          let display = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0 : false

                          let displayKPIBtn = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0KPI : false

                          let displayNewAtt = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL0 : false

                          let BtnAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0 : false

                          let BtnKPI = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0KPI : false

                          let BtnNewAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL0 : false


                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              CampaignName = c.Name,
                              Name = y.FullName,
                              UserID = y.Id,
                              FLID = y.FunctionalLeader.HasValue ? y.FunctionalLeader.Value : 0,
                              L0ID = y.Id,
                              L1ID = y.Manager.HasValue ? y.Manager.Value : 0,
                              L2ID = y.L2.HasValue ? y.L2.Value : 0,
                              isDisplayAttitudeBtn = display,
                              isDisplayKPIBtn = displayKPIBtn,
                              isDisplayNewAttBtn = displayNewAtt,
                              Type = "L0",
                              Dept = dept,
                              Center = center,
                              BtnAttitude = !BtnAttitude,
                              BtnKPI = !BtnKPI,
                              BtnNewAttitude = !BtnNewAttitude
                          }).Where(x => x.isDisplayNewAttBtn || x.isDisplayKPIBtn).ToList();
            return result;
            
        }

        //for L1
        public async Task<List<EvaluationDto>> GetFirstLevelAppraisal(int userID)
        {
            var list_evaluation = await _repoEvaluation.FindAll().ToListAsync();
            var list_ac = await _repoAccount.FindAll(x => x.Manager == userID && x.L0.Value).ToListAsync();
            var list_systemFlow = await _repoSystemFlow.FindAll().ToListAsync();
            var list_attSubmit = await _repoAttitudeSubmit.FindAll().ToListAsync();
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id
                          join z in list_systemFlow on y.SystemFlow equals z.SystemFlowID
                          join t in list_attSubmit on x.CampaignID equals t.CampaignID into xt
                          join c in list_campaign on x.CampaignID equals c.ID


                          //attitude
                          let displayFL = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnFL : false

                          let displayL0 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0 : false

                          let display = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? y.SystemFlow == 1 || y.SystemFlow == 4 ? false : xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1 : false

                          //kpi

                          let displayKPIBtnFL = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnFLKPI : false

                          let displayKPIBtnL0 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0KPI : false

                          let displayKPIBtn = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? y.SystemFlow == 1 || y.SystemFlow == 4 ? false : xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1KPI : false

                          //new-attitude
                          let displayNewAttFL = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttFL : false

                          let displayNewAttL0 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL0 : false

                          let displayNewAtt = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? y.SystemFlow == 1 || y.SystemFlow == 4 ? false : xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL1 : false
                          
                          // disable-enable button
                          let BtnAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1 : false

                          let BtnKPI = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1KPI : false

                          let BtnNewAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL1 : false

                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              CampaignName = c.Name,
                              Name = y.FullName,
                              UserID = y.Id,
                              FLID = y.FunctionalLeader.HasValue ? y.FunctionalLeader.Value : 0,
                              L0ID = y.Id,
                              L1ID = y.Manager.HasValue ? y.Manager.Value : 0,
                              L2ID = y.L2.HasValue ? y.L2.Value : 0,

                              isDisplayAttitudeBtnFL = displayFL,
                              isDisplayAttitudeBtnL0 = displayL0,
                              isDisplayAttitudeBtn = display,

                              isDisplayKPIBtnFL = displayKPIBtnFL,
                              isDisplayKPIBtnL0 = displayKPIBtnL0,
                              isDisplayKPIBtn = displayKPIBtn,

                              isDisplayNewAttBtnFL = displayNewAttFL,
                              isDisplayNewAttBtnL0 = displayNewAttL0,
                              isDisplayNewAttBtn = displayNewAtt,

                              Type = "L1",
                              Dept = dept,
                              Center = center,
                              BtnAttitude = !BtnAttitude,
                              BtnKPI = !BtnKPI,
                              BtnNewAttitude = !BtnNewAttitude
                          }).Where(x 
                          => x.isDisplayNewAttBtnFL == false
                          && x.isDisplayNewAttBtnL0 == false
                          && x.isDisplayKPIBtnFL == false
                          && x.isDisplayKPIBtnL0 == false
                          && (x.isDisplayNewAttBtn || x.isDisplayKPIBtn)).ToList();
            return result;
            
        }

        //for L2
        public async Task<List<EvaluationDto>> GetSecondLevelAppraisal(int userID)
        {

            var list_evaluation = await _repoEvaluation.FindAll().ToListAsync();
            var isGM = _repoAccount.FindById(userID).GM.Value;
            var list_ac = await _repoAccount.FindAll(x => x.L2 == userID && x.L0.Value).ToListAsync();
            var list_systemFlow = await _repoSystemFlow.FindAll().ToListAsync();
            var list_attSubmit = await _repoAttitudeSubmit.FindAll().ToListAsync();
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id
                          join z in list_systemFlow on y.SystemFlow equals z.SystemFlowID
                          join t in list_attSubmit on x.CampaignID equals t.CampaignID into xt
                          join c in list_campaign on x.CampaignID equals c.ID

                          //attitude
                          let displayFL = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnFL : false

                          let displayL0 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0 : false

                          let displayL1 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1 : false

                          let display = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? y.SystemFlow == 3 || y.SystemFlow == 6 ? false : xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL2 : false


                          //kpi

                          let displayKPIBtnFL = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnFLKPI : false

                          let displayKPIBtnL0 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0KPI : false

                          let displayKPIBtnL1 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1KPI : false

                          let displayKPIBtn = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? y.SystemFlow == 3 || y.SystemFlow == 6 ? false : xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL2KPI : false


                          //new-atttitude

                          let displayNewAttFL = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttFL : false

                          let displayNewAttL0 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL0 : false

                          let displayNewAttL1 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL1 : false

                          let displayNewAtt = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? y.SystemFlow == 3 || y.SystemFlow == 6 ? false : xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL2 : false

                          //disable button kpi - attitude
                          let BtnAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL2 : false

                          let BtnKPI = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL2KPI : false

                          let BtnNewAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL2 : false

                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              CampaignName = c.Name,
                              Name = y.FullName,
                              UserID = y.Id,
                              FLID = y.FunctionalLeader.HasValue ? y.FunctionalLeader.Value : 0,
                              L0ID = y.Id,
                              L1ID = y.Manager.HasValue ? y.Manager.Value : 0,
                              L2ID = y.L2.HasValue ? y.L2.Value : 0,

                              isDisplayAttitudeBtnFL = displayFL,
                              isDisplayAttitudeBtnL0 = displayL0,
                              isDisplayAttitudeBtnL1 = displayL1,
                              isDisplayAttitudeBtn = display,

                              isDisplayKPIBtnFL = displayKPIBtnFL,
                              isDisplayKPIBtnL0 = displayKPIBtnL0,
                              isDisplayKPIBtnL1 = displayKPIBtnL1,
                              isDisplayKPIBtn = displayKPIBtn,

                              isDisplayNewAttBtnFL= displayNewAttFL,
                              isDisplayNewAttBtnL0 = displayNewAttL0,
                              isDisplayNewAttBtnL1 = displayNewAttL1,
                              isDisplayNewAttBtn = displayNewAtt,

                              Type = "L2",
                              isGM = isGM,
                              Dept = dept,
                              Center = center,
                              BtnAttitude = !BtnAttitude,
                              BtnKPI = !BtnKPI,
                              BtnNewAttitude = !BtnNewAttitude
                          }).Where(x 
                          => 
                          x.isDisplayNewAttBtnFL == false 
                          && x.isDisplayNewAttBtnL0 == false
                          && x.isDisplayNewAttBtnL1== false
                          && x.isDisplayKPIBtnFL == false
                          && x.isDisplayKPIBtnL0 == false
                          && x.isDisplayKPIBtnL1 == false
                          && (x.isDisplayNewAttBtn || x.isDisplayKPIBtn)).ToList();
            return result;

          
        }

        //for Fl
        public async Task<List<EvaluationDto>> GetFLFeedback(int userID)
        {
            var list_evaluation = await _repoEvaluation.FindAll().ToListAsync();
            var list_ac = await _repoAccount.FindAll(x => x.FunctionalLeader == userID && x.L0.Value).ToListAsync();
            var list_systemFlow = await _repoSystemFlow.FindAll().ToListAsync();
            var list_attSubmit = await _repoAttitudeSubmit.FindAll().ToListAsync();
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id
                          join z in list_systemFlow on y.SystemFlow equals z.SystemFlowID
                          join t in list_attSubmit on x.CampaignID equals t.CampaignID into xt
                          join c in list_campaign on x.CampaignID equals c.ID
                          let display = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null 
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnFL : false
                          let displayNewAtt = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null 
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttFL : false
                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              CampaignName = c.Name,
                              Name = y.FullName,
                              UserID = y.Id,
                              FLID = y.FunctionalLeader.HasValue ? y.FunctionalLeader.Value : 0,
                              L0ID = y.Id,
                              L1ID = y.Manager.HasValue ? y.Manager.Value : 0,
                              L2ID = y.L2.HasValue ? y.L2.Value : 0,
                              isDisplayAttitudeBtn = display,
                              isDisplayNewAttBtn = displayNewAtt,
                              Type = "FL",
                              Dept = dept,

                              Center = center
                          }).Where(x => x.isDisplayNewAttBtn).ToList();
            return result;
        }

        public async Task<List<NewAttitudeDetailDto>> GetDetailNewAttitude(int campaignID, int scoreTo)
        {
            var account = _repoAccount.FindById(scoreTo);
            var accountCampaign = _repoAccountCampaign.FindAll(x => x.CampaignID == campaignID && x.AccountID == scoreTo).FirstOrDefault();
            var ListNewAttitudeDetail = (from a in await _repoNewAttContent.FindAll().ToListAsync()
                                        select new NewAttitudeScoreDetailDto() {
                                            OrderNumber = a.OrderNumber,
                                            Name = a.Name,
                                            Definition = a.Definition,
                                            Behavior = a.Behavior,
                                            // PointFL = GetPoint(campaignID, scoreTo, account.FunctionalLeader.ToInt(), a.ID, "FL"),
                                            // PointL0 = GetPoint(campaignID, scoreTo, scoreTo, a.ID, "L0"),
                                            // PointL1 = GetPoint(campaignID, scoreTo, account.Manager.ToInt(), a.ID, "L1"),
                                            // PointL2 = GetPoint(campaignID, scoreTo, account.L2.ToInt(), a.ID, "L2"),
                                            PointFL = GetPoint(campaignID, scoreTo, accountCampaign.FL, a.ID, "FL"),
                                            PointL0 = GetPoint(campaignID, scoreTo, scoreTo, a.ID, "L0"),
                                            PointL1 = GetPoint(campaignID, scoreTo, accountCampaign.L1, a.ID, "L1"),
                                            PointL2 = GetPoint(campaignID, scoreTo, accountCampaign.L2, a.ID, "L2"),
                                        }).ToList();

            var ListDetail = (from a in ListNewAttitudeDetail
                        group a by new {
                            a.OrderNumber,
                            a.Name,
                            a.Definition,
                        } into child
                        select new NewAttitudeDetailDto() {
                            OrderNumber = child.Key.OrderNumber,
                            Name = child.Key.Name,
                            Definition = child.Key.Definition,
                            Comment = _repoNewAttAttchment.FindAll(x => x.CampaignID == campaignID && x.ScoreFrom == scoreTo && x.ScoreTo == scoreTo && x.OrderNumber == child.Key.OrderNumber).FirstOrDefault()
                                                    != null ? _repoNewAttAttchment.FindAll(x => x.CampaignID == campaignID && x.ScoreFrom == scoreTo && x.ScoreTo == scoreTo && x.OrderNumber == child.Key.OrderNumber).FirstOrDefault().Comment : "",
                            NewAttitudeScoreDetai = child.Select(y => new NewAttitudeScoreDetailDto() {
                                                ID = y.ID,
                                                PointFL = y.PointFL,
                                                PointL0 = y.PointL0,
                                                PointL1 = y.PointL1,
                                                PointL2 = y.PointL2,
                                                Behavior = y.Behavior
                                            }).ToList()
                        }).ToList();

            return ListDetail;
            
        }

        //Detail Score
        public int GetPoint(int campaignID, int scoreTo, int scoreFrom, int attitudeContenID, string type)
        {
            int point = 0;
            var NewAttitudeScore = (from a in _repoNewAttScore.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == scoreFrom && x.AttitudeContenID == attitudeContenID)
                                        join b in _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && (type == "FL" ? x.IsSubmitAttitudeFL == true : type == "L0" ? x.IsSubmitAttitudeL0 == true : type == "L1" ? x.IsSubmitAttitudeL1 == true : x.IsSubmitAttitudeL2 == true) ) on a.ScoreTo equals b.SubmitTo
                                        select new NewAttitudeScoreDto() {
                                                    ID = a.ID,
                                                    Point1 = a.Point1,
                                                    Point2 = a.Point2,
                                                    Point3 = a.Point3,
                                                    Point4 = a.Point4,
                                                    Point5 = a.Point5,
                                                    Point6 = a.Point6,
                                                    Point7 = a.Point7,
                                                    Point8 = a.Point8,
                                                    Point9 = a.Point9,
                                                    Point10 = a.Point10
                                        }).FirstOrDefault();

            if (NewAttitudeScore == null)
            {
                return point = 0;
            }
            point = NewAttitudeScore switch
                {
                    { Point1: true } => 1,
                    { Point2: true } => 2,
                    { Point3: true } => 3,
                    { Point4: true } => 4,
                    { Point5: true } => 5,
                    { Point6: true } => 6,
                    { Point7: true } => 7,
                    { Point8: true } => 8,
                    { Point9: true } => 9,
                    { Point10: true } => 10,
                    _ => 0
                };
            
            return point;
            
        }

        public async Task<NewAttitudeEvaluationDetailDto> GetDetailNewAttitudeEvaluation(int campaignID, int scoreTo)
        {
            var account = _repoAccount.FindById(scoreTo);
            var accountCampaign = _repoAccountCampaign.FindAll(x => x.CampaignID == campaignID && x.AccountID == scoreTo).FirstOrDefault();
            if (account == null)
            {
                return new NewAttitudeEvaluationDetailDto();
            }

            var newAttitudeEvaluationL0 = (from a in await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == scoreTo).ToListAsync()
                                            join b in await _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.IsSubmitAttitudeL0).ToListAsync() on a.ScoreTo equals b.SubmitTo
                                            select a).FirstOrDefault();

            var newAttitudeEvaluationL1 = (from a in await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == accountCampaign.L1).ToListAsync()
                                            join b in await _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.IsSubmitAttitudeL1).ToListAsync() on a.ScoreTo equals b.SubmitTo
                                            select a).FirstOrDefault();

            var newAttitudeEvaluationL2 = (from a in await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == accountCampaign.L2).ToListAsync()
                                            join b in await _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.IsSubmitAttitudeL2).ToListAsync() on a.ScoreTo equals b.SubmitTo
                                            select a).FirstOrDefault();

            var newAttitudeEvaluationFL = (from a in await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == accountCampaign.FL).ToListAsync()
                                            join b in await _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.IsSubmitAttitudeFL).ToListAsync() on a.ScoreTo equals b.SubmitTo
                                            select a).FirstOrDefault();

            var newAttitudeEvaluation = new NewAttitudeEvaluationDetailDto();
            // L0
            newAttitudeEvaluation.FirstQuestion1L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FirstQuestion1 : false;
            newAttitudeEvaluation.FirstQuestion2L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FirstQuestion2 : false;
            newAttitudeEvaluation.FirstQuestion3L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FirstQuestion3 : false;
            newAttitudeEvaluation.FirstQuestion4L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FirstQuestion4 : false;
            newAttitudeEvaluation.FirstQuestion5L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FirstQuestion5 : false;
            newAttitudeEvaluation.FirstQuestion6L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FirstQuestion6 : false;

            newAttitudeEvaluation.SecondQuestion1L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.SecondQuestion1 : false;
            newAttitudeEvaluation.SecondQuestion2L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.SecondQuestion2 : false;
            newAttitudeEvaluation.SecondQuestion3L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.SecondQuestion3 : false;
            newAttitudeEvaluation.SecondQuestion4L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.SecondQuestion4 : false;
            newAttitudeEvaluation.SecondQuestion5L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.SecondQuestion5 : false;
            newAttitudeEvaluation.SecondQuestion6L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.SecondQuestion6 : false;

            newAttitudeEvaluation.ThirdQuestionL0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.ThirdQuestion : "";
            newAttitudeEvaluation.FourthQuestionL0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FourthQuestion : "";

            // L1
            newAttitudeEvaluation.FirstQuestion1L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.FirstQuestion1 : false;
            newAttitudeEvaluation.FirstQuestion2L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.FirstQuestion2 : false;
            newAttitudeEvaluation.FirstQuestion3L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.FirstQuestion3 : false;
            newAttitudeEvaluation.FirstQuestion4L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.FirstQuestion4 : false;
            newAttitudeEvaluation.FirstQuestion5L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.FirstQuestion5 : false;
            newAttitudeEvaluation.FirstQuestion6L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.FirstQuestion6 : false;

            newAttitudeEvaluation.SecondQuestion1L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.SecondQuestion1 : false;
            newAttitudeEvaluation.SecondQuestion2L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.SecondQuestion2 : false;
            newAttitudeEvaluation.SecondQuestion3L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.SecondQuestion3 : false;
            newAttitudeEvaluation.SecondQuestion4L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.SecondQuestion4 : false;
            newAttitudeEvaluation.SecondQuestion5L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.SecondQuestion5 : false;
            newAttitudeEvaluation.SecondQuestion6L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.SecondQuestion6 : false;

            newAttitudeEvaluation.ThirdQuestionL1 = newAttitudeEvaluationL1 != null ?  newAttitudeEvaluationL1.ThirdQuestion : "";
            
            // L2, FL
            newAttitudeEvaluation.CommentL2 = newAttitudeEvaluationL2 != null ? newAttitudeEvaluationL2.ThirdQuestion : "";
            newAttitudeEvaluation.CommentFL = newAttitudeEvaluationFL != null ? newAttitudeEvaluationFL.ThirdQuestion : "";

            return newAttitudeEvaluation;
            
        }

    }
}
