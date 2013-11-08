using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LuaInterface;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Lua.Properties;

namespace Realm.Library.Lua
{
    /// <summary>
    ///
    /// </summary>
    public sealed class LuaVirtualMachine : Entity
    {
        private readonly Dictionary<Task, CancellationTokenSource> _tasks;
        private readonly LogWrapper _log;
        private readonly LuaFunctionRepository _repository;
        private readonly LuaInterfaceProxy _luaProxy;

        ///  <summary>
        /// 
        ///  </summary>
        ///  <param name="id"></param>
        ///  <param name="log"></param>
        ///  <param name="repository"></param>
        /// <param name="luaProxy"></param>
        public LuaVirtualMachine(long id, LogWrapper log, LuaFunctionRepository repository, LuaInterfaceProxy luaProxy)
            : base(id, "LuaVirtualMachine" + id)
        {
            Validation.Validate<ArgumentOutOfRangeException>(id >= 1 && id <= Int64.MaxValue);
            Validation.IsNotNull(log, "log");
            Validation.IsNotNull(repository, "repository");

            _tasks = new Dictionary<Task, CancellationTokenSource>();
            _log = log;
            _repository = repository;
            _luaProxy = luaProxy;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="script"></param>
        public void ExecuteScript(LuaScript script)
        {
            _log.InfoFormat("Script {0} submitted.", script.Name);

            Task<LuaResponse> task = null;
            CancellationTokenSource tokenSource = null;

            try
            {
                tokenSource = new CancellationTokenSource();
                task = Task.Factory.StartNew(() => ExecuteNextScript(tokenSource.Token, script), tokenSource.Token);
                if (task.IsNull())
                    throw new ObjectDisposedException("task");

                _tasks.Add(task, tokenSource);
                task.Wait();

                // Do something with result
            }
            catch (AggregateException ex)
            {
                _log.InfoFormat("Script {0} cancelled.", script.Name);
                ex.InnerException.Handle(ExceptionHandlingOptions.RecordAndThrow, _log);
            }
            catch (Exception ex)
            {
                ex.Handle(ExceptionHandlingOptions.RecordAndThrow, _log);
            }
            finally
            {
                // ReSharper disable AssignNullToNotNullAttribute
                if (task.IsNotNull())
                {
                    if (_tasks.ContainsKey(task))

                        _tasks.Remove(task);
                    task.Cancel(tokenSource);
                }
                // ReSharper restore AssignNullToNotNullAttribute
            }
        }

        /// <summary>
        /// Tells the virtual machine to re-register functions
        /// </summary>
        public void RegisterFunctions()
        {
            _repository.Values.ToList().ForEach(x => _luaProxy.RegisterFunction(x.Name, null, x.Info));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var it = _tasks.GetEnumerator();
                while (it.MoveNext())
                {
                    _tasks.Remove(it.Current.Key);
                    it.Current.Key.Cancel(it.Current.Value);
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        private LuaResponse ExecuteNextScript(CancellationToken token, LuaScript script)
        {
            token.ThrowIfCancellationRequested();

            bool returnVal = false;
            var responseList = new List<object>();

            try
            {
                object[] responseData = script.IsFile
                                            ? _luaProxy.DoFile(script.Script)
                                            : _luaProxy.DoString(script.Script);
                responseList = responseData.IsNotNull() ? responseData.ToList() : new List<object>();

                if (!string.IsNullOrEmpty(script.Function))
                {
                    var luaFunction = _luaProxy.GetFunction(script.Function);
                    if (luaFunction.IsNull())
                        throw new LuaException(string.Format(Resources.ERR_FUNC_NOT_FOUND, script.Function, script.Name));

                    responseData = luaFunction.Call(script.Table);
                    if (responseData.IsNotNull())
                        responseList.AddRange(responseData.ToList());
                }
                returnVal = true;
            }
            catch (LuaException ex)
            {
                ex.Handle(ExceptionHandlingOptions.RecordAndThrow, _log);
            }

            return new LuaResponse
            {
                Success = returnVal,
                ResponseData = responseList.IsNotNull() ? responseList.ToList() : new List<object>()
            };
        }
    }
}