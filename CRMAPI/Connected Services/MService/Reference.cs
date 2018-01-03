﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRMAPI.MService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MService.MobileServiceSoap")]
    public interface MobileServiceSoap {
        
        // CODEGEN: 命名空間 http://tempuri.org/ 的元素名稱  logicalName 未標示為 nillable，正在產生訊息合約
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SyncMobile", ReplyAction="*")]
        CRMAPI.MService.SyncMobileResponse SyncMobile(CRMAPI.MService.SyncMobileRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SyncMobile", ReplyAction="*")]
        System.Threading.Tasks.Task<CRMAPI.MService.SyncMobileResponse> SyncMobileAsync(CRMAPI.MService.SyncMobileRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SyncMobileRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SyncMobile", Namespace="http://tempuri.org/", Order=0)]
        public CRMAPI.MService.SyncMobileRequestBody Body;
        
        public SyncMobileRequest() {
        }
        
        public SyncMobileRequest(CRMAPI.MService.SyncMobileRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncMobileRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string logicalName;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string parameter;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string function;
        
        public SyncMobileRequestBody() {
        }
        
        public SyncMobileRequestBody(string logicalName, string parameter, string function) {
            this.logicalName = logicalName;
            this.parameter = parameter;
            this.function = function;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SyncMobileResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SyncMobileResponse", Namespace="http://tempuri.org/", Order=0)]
        public CRMAPI.MService.SyncMobileResponseBody Body;
        
        public SyncMobileResponse() {
        }
        
        public SyncMobileResponse(CRMAPI.MService.SyncMobileResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncMobileResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string SyncMobileResult;
        
        public SyncMobileResponseBody() {
        }
        
        public SyncMobileResponseBody(string SyncMobileResult) {
            this.SyncMobileResult = SyncMobileResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface MobileServiceSoapChannel : CRMAPI.MService.MobileServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MobileServiceSoapClient : System.ServiceModel.ClientBase<CRMAPI.MService.MobileServiceSoap>, CRMAPI.MService.MobileServiceSoap {
        
        public MobileServiceSoapClient() {
        }
        
        public MobileServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MobileServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MobileServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MobileServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CRMAPI.MService.SyncMobileResponse CRMAPI.MService.MobileServiceSoap.SyncMobile(CRMAPI.MService.SyncMobileRequest request) {
            return base.Channel.SyncMobile(request);
        }
        
        public string SyncMobile(string logicalName, string parameter, string function) {
            CRMAPI.MService.SyncMobileRequest inValue = new CRMAPI.MService.SyncMobileRequest();
            inValue.Body = new CRMAPI.MService.SyncMobileRequestBody();
            inValue.Body.logicalName = logicalName;
            inValue.Body.parameter = parameter;
            inValue.Body.function = function;
            CRMAPI.MService.SyncMobileResponse retVal = ((CRMAPI.MService.MobileServiceSoap)(this)).SyncMobile(inValue);
            return retVal.Body.SyncMobileResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CRMAPI.MService.SyncMobileResponse> CRMAPI.MService.MobileServiceSoap.SyncMobileAsync(CRMAPI.MService.SyncMobileRequest request) {
            return base.Channel.SyncMobileAsync(request);
        }
        
        public System.Threading.Tasks.Task<CRMAPI.MService.SyncMobileResponse> SyncMobileAsync(string logicalName, string parameter, string function) {
            CRMAPI.MService.SyncMobileRequest inValue = new CRMAPI.MService.SyncMobileRequest();
            inValue.Body = new CRMAPI.MService.SyncMobileRequestBody();
            inValue.Body.logicalName = logicalName;
            inValue.Body.parameter = parameter;
            inValue.Body.function = function;
            return ((CRMAPI.MService.MobileServiceSoap)(this)).SyncMobileAsync(inValue);
        }
    }
}
