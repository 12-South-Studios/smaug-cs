using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LuaInterface;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Lua.Properties;

namespace Realm.Library.Lua
{
    /// <summary>
    ///
    /// </summary>
    public sealed class LuaVirtualMachineContext : Entity
    {
        private readonly ConcurrentQueue<LuaVirtualMachine> _virtualMachines = new ConcurrentQueue<LuaVirtualMachine>();
        private readonly LuaScriptCache _cache;
        private readonly LuaFunctionRepository _repository;
        private readonly LuaInterfaceProxy _proxy;

        /// <summary>
        /// Constructor
        /// </summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope",
                 Justification = "VirtualMachines are disposed in Context finalizer")]
        public LuaVirtualMachineContext(LogWrapper log, int numberVirtualMachines, LuaFunctionRepository repository)
            : base(1, "LuaVirtualMachineContext")
        {
            Validation.IsNotNull(log, "log");
            Validation.Validate<ArgumentOutOfRangeException>(numberVirtualMachines > 0 && numberVirtualMachines <= Int32.MaxValue);
            Validation.IsNotNull(repository, "repository");

            _repository = repository;
            _cache = new LuaScriptCache();
            _proxy = new LuaInterfaceProxy();
            Enumerable.Range(0, numberVirtualMachines)
                      .ToList()
                      .ForEach(i => _virtualMachines.Enqueue(new LuaVirtualMachine(i + 1, log, repository, _proxy)));
        }

        /// <summary>
        /// Gets the number of virtual machines on the queue
        /// </summary>
        public int VMCount { get { return _virtualMachines.Count; } }

        /// <summary>
        /// Gets the next virtual machine on the queue and then re-enqueues the chosen vm.
        /// </summary>
        /// <returns></returns>
        public LuaVirtualMachine NextVM
        {
            get
            {
                LuaVirtualMachine vm;

                _virtualMachines.TryDequeue(out vm);

                if (vm.IsNotNull())
                    _virtualMachines.Enqueue(vm);

                return vm;
            }
        }

        /// <summary>
        /// Registers all lua functions found within the given object
        /// </summary>
        /// <param name="value">Object to scan for LuaFunctionAttribute</param>
        public void RegisterLuaFunctions(object value)
        {
            Validation.IsNotNull(value, "value");

            RegisterLuaFunctions(value.GetType());
        }

        /// <summary>
        /// Registers all lua functions found within the given object type
        /// </summary>
        /// <param name="type">The type of the object to scan for LuaFunctionAttribute</param>
        public void RegisterLuaFunctions(Type type)
        {
            Validation.IsNotNull(type, "type");

            try
            {
                // Get the target type
                var targetType = type;

                // ... and simply iterate through all it's methods
                targetType.GetMethods().ToList().ForEach(info => Attribute.GetCustomAttributes(info).ToList().ForEach(attr =>
                    {
                        // and if they happen to be one of our LuaFunctionAttribute attributes
                        if (attr.GetType() != typeof(LuaFunctionAttribute)) return;
                        var functionAttr = attr as LuaFunctionAttribute;
                        var paramTable = new Dictionary<string, string>();

                        // Get the desired function name and doc string, along with parameter info
                        if (functionAttr == null) return;

                        var strFName = functionAttr.Name;
                        var strFDoc = functionAttr.Description;
                        var paramDocs = functionAttr.Parameters;

                        // Now get the expected parameters from the MethodInfo object
                        var paramInfo = info.GetParameters();

                        // If they don't match, someone forgot to add some documentation to the
                        // attribute, complain and go to the next method
                        if (paramDocs.IsNotNull() && (paramInfo.Length != paramDocs.Count))
                        {
                            throw new ArgumentException(String.Format(Resources.ERR_FUNC_ARG_MISMATCH,
                                                                      info.Name, strFName, paramDocs.Count,
                                                                      paramInfo.Length));
                        }

                        // Build a parameter <-> parameter doc dictionary
                        if (paramDocs.IsNotNull())
                            Enumerable.Range(0, paramInfo.Length)
                                      .ToList()
                                      .ForEach(i => paramTable.Add(paramInfo[i].Name, paramDocs.ElementAt(i)));

                        _repository.Add(strFName, new LuaFunctionDescriptor(strFName, strFDoc, paramTable, info));
                    }));

                _virtualMachines.ToList().ForEach(x => x.RegisterFunctions());
            }
            catch (ArgumentException ex)
            {
                ex.Handle<LuaException>(ExceptionHandlingOptions.RecordAndThrow);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                while (_virtualMachines.Count > 0)
                {
                    LuaVirtualMachine vm;
                    _virtualMachines.TryDequeue(out vm);
                    vm.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}