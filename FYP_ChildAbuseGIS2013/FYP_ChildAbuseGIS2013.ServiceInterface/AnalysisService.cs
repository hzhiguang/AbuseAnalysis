﻿using System;
using System.Net;
using System.Collections.Generic;
using FYP_ChildAbuseGIS2013.ServiceModel.Types;
using FYP_ChildAbuseGIS2013.ServiceModel.Operations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.Common.Web;

namespace FYP_ChildAbuseGIS2013.ServiceInterface
{
    public class AnalysisService : Service
    {
        public AnalysisResult Get(Analysiss request)
        {
            return new AnalysisResult { Analysis = Db.Select<Analysis>() };
        }

        public CreateAnalysisResult Post(CreateAnalysis request)
        {
            Db.ExecuteSql("INSERT INTO analysis (totalframe, smileframe, angryframe, sadframe, neutralframe, leftfistframe, rightfistframe, leftpalmframe, rightpalmframe) Values ('" + request.totalframe + "','" + request.smileframe + "','" + request.angryframe + "', '" + request.sadframe + "', '" + request.neutralframe + "', '" + request.leftfistframe + "', '" + request.rightfistframe + "', '" + request.leftpalmframe + "', '" + request.rightpalmframe + "')");
            long id = Db.GetLastInsertId();
            var analysis = Db.GetByIdOrDefault<Analysis>(id);
            if (analysis == null)
            {
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Analysis does not exist: " + id.ToString()));
            }
            return new CreateAnalysisResult { Analysis = analysis };
        }

        public AnalysisDetailsResult Get(GetAnalysisDetails request)
        {
            var analysis = Db.GetByIdOrDefault<Analysis>(request.ID);
            if (analysis == null)
            {
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Analysis does not exist: " + request.ID));
            }
            return new AnalysisDetailsResult { Analysis = analysis };
        }
    }
}