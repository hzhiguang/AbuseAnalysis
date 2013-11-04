using System;
using System.Net;
using System.Collections.Generic;
using FYP_ChildAbuseGIS2013.ServiceModel.Types;
using FYP_ChildAbuseGIS2013.ServiceModel.Operations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.Common.Web;

namespace FYP_ChildAbuseGIS2013.ServiceInterface
{
    public class AnalysisDetailsService : Service
    {
        public AnalysisDetailsResult Get(AnalysisDetailsOperation request)
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
