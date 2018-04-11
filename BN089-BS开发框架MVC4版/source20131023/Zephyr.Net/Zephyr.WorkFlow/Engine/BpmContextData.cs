using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Reflection;
using Zephyr.Data;

namespace Zephyr.WorkFlow
{
    public partial class BpmContext
    {
        private int save<T>(T model) where T : IModel
        {
            model.UpdateDate = DateTime.Now;
            model.UpdatePerson = _currentActor;
            var rows = _db.Update<T>(typeof(T).Name, model).AutoMap(x => x.Id).Where(x => x.Id).Execute();

            if (rows == 0)
            {
                model.CreateDate = DateTime.Now;
                model.CreatePerson = _currentActor;
            }
            rows = _db.Insert<T>(typeof(T).Name, model).AutoMap(x => x.Id).Execute();

            return rows;
        }

        private wf_processDefinition loadDataProcessDefinition(string processDefinitionName)
        {
            var dataProcessDefinition = _db.Sql(@"
select top 1 * 
from wf_processDefinition 
where name=@0 
order by version desc"
                , processDefinitionName)
                .QuerySingle<wf_processDefinition>();
            return dataProcessDefinition;
        }

        private wf_processDefinition loadDataProcessDefinition(int processDefinitionId)
        {
            var dataProcessDefinition = _db.Sql(@"
select * 
from wf_processDefinition 
where Id=@0 ", processDefinitionId)
                .QuerySingle<wf_processDefinition>();
            return dataProcessDefinition;
        }

        private wf_processInstance loadDataProcessInstance(int processInstanceId)
        {
            var dataProcessInstance = _db.Sql(@"
select * 
from wf_processInstance 
where Id=@0 ", processInstanceId)
             .QuerySingle<wf_processInstance>();

            return dataProcessInstance;
        }
 
        internal int getProcessDefinitionNextVersion(string processDefinitionName) 
        {
            var result = _db.Sql(@"
select isnull(max(Version),0)+1 
from wf_processDefinition 
where name = @0", processDefinitionName)
                    .QuerySingle<int>();

            return result;
        }

        public wf_taskInstance loadDataTaskInstance(int taskInstanceId)
        {
            var result = _db.Sql(@"
select *
from wf_taskInstance 
where Id = @0 ", taskInstanceId)
            .QuerySingle<wf_taskInstance>();

            return result;
        }

        public wf_taskInstance loadDataTaskInstanceFirstRun(int processInstanceId, string task)
        {
            var result = _db.Sql(@"
select top 1 *
from wf_taskInstance 
where ProcessInstanceId = @0
and Task = @1
and TaskState = @2 ", processInstanceId
                    , task
                    , TaskState.Run.ToString())
            .QuerySingle<wf_taskInstance>();

            return result;
        }

        public wf_taskInstance loadDataTaskInstanceFirst(int processInstanceId, string task)
        {
            var result = _db.Sql(@"
select top 1 *
from wf_taskInstance 
where ProcessInstanceId = @0
and Task = @1", processInstanceId
                    , task)
            .QuerySingle<wf_taskInstance>();

            return result;
        }

        public wf_token loadDataToken(int tokenId)
        {
            var result = _db.Sql(@"
select *
from wf_token 
where Id = @0", tokenId)
            .QuerySingle<wf_token>();

            return result;
        }

        public wf_token loadDataTokenActiveByTaskInstanceId(int taskInstanceId)
        {
            var result = _db.Sql(@"
select top 1 *
from wf_token 
where TaskInstanceId = @0
and TokenState =  @1", taskInstanceId, TokenState.Activate.ToString())
            .QuerySingle<wf_token>();

            return result;
        }

        public List<wf_token> loadDataJoinParentTokensByPackageId(int? packageId)
        {
            var result = _db.Sql(@"
select * from wf_token
where id in (
	select A.parentId
	from wf_token A
	where A.packageId = @0
	group by A.parentId
	having  (count(*) = (select count(1) from wf_token B where B.parentId = A.parentId))
)", packageId)
         .QueryMany<wf_token>();

            return result;

        }

        public int updateTokenStateByParentId(int? parentId,TokenState tokenTask)
        {
            var result = _db.Update("wf_token")
                .Column("TokenState", tokenTask.ToString())
                .Where("ParentId", parentId)
                .Execute();

            return result;
        }

        public List<wf_transitionInstance> loadDataArrivedTransitionInstance(int toTaskInstanceId)
        {
            var result = _db.Sql(@"
select *
from wf_transitionInstance 
where ToTaskInstanceId = @0
and IsExcuted = 1", toTaskInstanceId)
            .QueryMany<wf_transitionInstance>();

            return result;
        }
    }
}
