/*
Copyright 2012, KISSY UI Library v1.30rc
MIT Licensed
build time: Nov 9 16:34
*/
/**
 * @ignore
 * @fileOverview A seed where KISSY grows up from , KISS Yeah !
 * @author lifesinger@gmail.com, yiminghe@gmail.com
 */
(function(S,undefined){function hasOwnProperty(o,p){return Object.prototype.hasOwnProperty.call(o,p);}
var host=this,MIX_CIRCULAR_DETECTION='__MIX_CIRCULAR',hasEnumBug=!({toString:1}.propertyIsEnumerable('toString')),enumProperties=['hasOwnProperty','isPrototypeOf','propertyIsEnumerable','toString','toLocaleString','valueOf'],meta={mix:function(r,s,ov,wl,deep){if(typeof ov==='object'){wl=ov['whitelist'];deep=ov['deep'];ov=ov['overwrite'];}
var cache=[],c,i=0;mixInternal(r,s,ov,wl,deep,cache);while(c=cache[i++]){delete c[MIX_CIRCULAR_DETECTION];}
return r;}},mixInternal=function(r,s,ov,wl,deep,cache){if(!s||!r){return r;}
if(ov===undefined){ov=true;}
var i=0,p,len;s[MIX_CIRCULAR_DETECTION]=r;cache.push(s);if(wl&&(len=wl.length)){for(;i<len;i++){p=wl[i];if(p in s){_mix(p,r,s,ov,wl,deep,cache);}}}else{for(p in s){if(p!=MIX_CIRCULAR_DETECTION){_mix(p,r,s,ov,wl,deep,cache);}}
if(hasEnumBug){for(;p=enumProperties[i++];){if(hasOwnProperty(s,p)){_mix(p,r,s,ov,wl,deep,cache);}}}}
return r;},_mix=function(p,r,s,ov,wl,deep,cache){if(ov||!(p in r)||deep){var target=r[p],src=s[p];if(target===src){return;}
if(deep&&src&&(S.isArray(src)||S.isPlainObject(src))){if(src[MIX_CIRCULAR_DETECTION]){r[p]=src[MIX_CIRCULAR_DETECTION];}else{var clone=target&&(S.isArray(target)||S.isPlainObject(target))?target:(S.isArray(src)?[]:{});r[p]=clone;mixInternal(clone,src,ov,wl,true,cache);}}else if(src!==undefined&&(ov||!(p in r))){r[p]=src;}}},seed=(host&&host[S])||{},guid=0,EMPTY='';seed.Env=seed.Env||{};host=seed.Env.host||(seed.Env.host=host||{});S=host[S]=meta.mix(seed,meta);S.mix(S,{configs:(S.configs||{}),version:'1.30rc',merge:function(var_args){var_args=S.makeArray(arguments);var o={},i,l=var_args.length;for(i=0;i<l;i++){S.mix(o,var_args[i]);}
return o;},augment:function(r,s1){var args=S.makeArray(arguments),len=args.length-2,i=1,ov=args[len],wl=args[len+1];if(!S.isArray(wl)){ov=wl;wl=undefined;len++;}
if(!S.isBoolean(ov)){ov=undefined;len++;}
for(;i<len;i++){S.mix(r.prototype,args[i].prototype||args[i],ov,wl);}
return r;},extend:function(r,s,px,sx){if(!s||!r){return r;}
var create=Object.create?function(proto,c){return Object.create(proto,{constructor:{value:c}});}:function(proto,c){function F(){}
F.prototype=proto;var o=new F();o.constructor=c;return o;},sp=s.prototype,rp;rp=create(sp,r);r.prototype=S.mix(rp,r.prototype);r.superclass=create(sp,s);if(px){S.mix(rp,px);}
if(sx){S.mix(r,sx);}
return r;},namespace:function(){var args=S.makeArray(arguments),l=args.length,o=null,i,j,p,global=(args[l-1]===true&&l--);for(i=0;i<l;i++){p=(EMPTY+args[i]).split('.');o=global?host:this;for(j=(host[p[0]]===o)?1:0;j<p.length;++j){o=o[p[j]]=o[p[j]]||{};}}
return o;},config:function(configName,configValue){var cfg,r,self=this,fn,Config=S.Config,configs=S.configs;if(S.isObject(configName)){S.each(configName,function(configValue,p){fn=configs[p];if(fn){fn.call(self,configValue);}else{Config[p]=configValue;}});}else{cfg=configs[configName];if(configValue===undefined){if(cfg){r=cfg.call(self);}else{r=Config[configName];}}else{if(cfg){r=cfg.call(self,configValue);}else{Config[configName]=configValue;}}}
return r;},log:function(msg,cat,src){if(S.Config.debug&&msg){if(src){msg=src+': '+msg;}
if(host['console']!==undefined&&console.log){console[cat&&console[cat]?cat:'log'](msg);}}},error:function(msg){if(S.Config.debug){throw msg;}},guid:function(pre){return(pre||EMPTY)+guid++;},keys:function(o){var result=[];for(var p in o){if(o.hasOwnProperty(p)){result.push(p);}}
if(hasEnumBug){S.each(enumProperties,function(name){if(hasOwnProperty(o,name)){result.push(name);}});}
return result;}});(function(){var c;S.Env=S.Env||{};S.Env.nodejs=(typeof require!=='undefined')&&(typeof exports!=='undefined');c=S.Config=S.Config||{};c.debug='@DEBUG@';S.__BUILD_TIME='20121109163450';})();return S;})('KISSY',undefined);(function(S,undefined){function hasOwnProperty(o,p){return Object.prototype.hasOwnProperty.call(o,p);}
var TRUE=true,FALSE=false,OP=Object.prototype,toString=OP.toString,AP=Array.prototype,indexOf=AP.indexOf,lastIndexOf=AP.lastIndexOf,filter=AP.filter,every=AP.every,some=AP.some,trim=String.prototype.trim,map=AP.map,EMPTY='',HEX_BASE=16,CLONE_MARKER='__~ks_cloned',COMPARE_MARKER='__~ks_compared',STAMP_MARKER='__~ks_stamped',RE_TRIM=/^[\s\xa0]+|[\s\xa0]+$/g,encode=encodeURIComponent,decode=decodeURIComponent,SEP='&',EQ='=',class2type={},htmlEntities={'&amp;':'&','&gt;':'>','&lt;':'<','&#x60;':'`','&#x2F;':'/','&quot;':'"','&#x27;':"'"},reverseEntities={},escapeReg,unEscapeReg,escapeRegExp=/[\-#$\^*()+\[\]{}|\\,.?\s]/g;(function(){for(var k in htmlEntities){if(htmlEntities.hasOwnProperty(k)){reverseEntities[htmlEntities[k]]=k;}}})();function getEscapeReg(){if(escapeReg){return escapeReg}
var str=EMPTY;S.each(htmlEntities,function(entity){str+=entity+'|';});str=str.slice(0,-1);return escapeReg=new RegExp(str,'g');}
function getUnEscapeReg(){if(unEscapeReg){return unEscapeReg}
var str=EMPTY;S.each(reverseEntities,function(entity){str+=entity+'|';});str+='&#(\\d{1,5});';return unEscapeReg=new RegExp(str,'g');}
function isValidParamValue(val){var t=typeof val;return val==null||(t!=='object'&&t!=='function');}
S.mix(S,{stamp:function(o,readOnly,marker){if(!o){return o}
marker=marker||STAMP_MARKER;var guid=o[marker];if(guid){return guid;}else if(!readOnly){try{guid=o[marker]=S.guid(marker);}
catch(e){guid=undefined;}}
return guid;},noop:function(){},type:function(o){return o==null?String(o):class2type[toString.call(o)]||'object';},isNull:function(o){return o===null;},isUndefined:function(o){return o===undefined;},isEmptyObject:function(o){for(var p in o){if(p!==undefined){return FALSE;}}
return TRUE;},isPlainObject:function(o){return o&&toString.call(o)==='[object Object]'&&'isPrototypeOf'in o;},equals:function(a,b,mismatchKeys,mismatchValues){mismatchKeys=mismatchKeys||[];mismatchValues=mismatchValues||[];if(a===b){return TRUE;}
if(a===undefined||a===null||b===undefined||b===null){return a==null&&b==null;}
if(a instanceof Date&&b instanceof Date){return a.getTime()==b.getTime();}
if(S.isString(a)&&S.isString(b)){return(a==b);}
if(S.isNumber(a)&&S.isNumber(b)){return(a==b);}
if(typeof a==='object'&&typeof b==='object'){return compareObjects(a,b,mismatchKeys,mismatchValues);}
return(a===b);},clone:function(input,filter){var memory={},ret=cloneInternal(input,filter,memory);S.each(memory,function(v){v=v.input;if(v[CLONE_MARKER]){try{delete v[CLONE_MARKER];}catch(e){S.log('delete CLONE_MARKER error : ');v[CLONE_MARKER]=undefined;}}});memory=null;return ret;},trim:trim?function(str){return str==null?EMPTY:trim.call(str);}:function(str){return str==null?EMPTY:str.toString().replace(RE_TRIM,EMPTY);},substitute:function(str,o,regexp){if(!S.isString(str)||!S.isPlainObject(o)){return str;}
return str.replace(regexp||/\\?\{([^{}]+)\}/g,function(match,name){if(match.charAt(0)==='\\'){return match.slice(1);}
return(o[name]===undefined)?EMPTY:o[name];});},each:function(object,fn,context){if(object){var key,val,i=0,length=object&&object.length,isObj=length===undefined||S.type(object)==='function';context=context||null;if(isObj){for(key in object){if(fn.call(context,object[key],key,object)===FALSE){break;}}}else{for(val=object[0];i<length&&fn.call(context,val,i,object)!==FALSE;val=object[++i]){}}}
return object;},indexOf:indexOf?function(item,arr){return indexOf.call(arr,item);}:function(item,arr){for(var i=0,len=arr.length;i<len;++i){if(arr[i]===item){return i;}}
return-1;},lastIndexOf:(lastIndexOf)?function(item,arr){return lastIndexOf.call(arr,item);}:function(item,arr){for(var i=arr.length-1;i>=0;i--){if(arr[i]===item){break;}}
return i;},unique:function(a,override){var b=a.slice();if(override){b.reverse();}
var i=0,n,item;while(i<b.length){item=b[i];while((n=S.lastIndexOf(item,b))!==i){b.splice(n,1);}
i+=1;}
if(override){b.reverse();}
return b;},inArray:function(item,arr){return S.indexOf(item,arr)>-1;},filter:filter?function(arr,fn,context){return filter.call(arr,fn,context||this);}:function(arr,fn,context){var ret=[];S.each(arr,function(item,i,arr){if(fn.call(context||this,item,i,arr)){ret.push(item);}});return ret;},map:map?function(arr,fn,context){return map.call(arr,fn,context||this);}:function(arr,fn,context){var len=arr.length,res=new Array(len);for(var i=0;i<len;i++){var el=S.isString(arr)?arr.charAt(i):arr[i];if(el||i in arr){res[i]=fn.call(context||this,el,i,arr);}}
return res;},reduce:function(arr,callback,initialValue){var len=arr.length;if(typeof callback!=='function'){throw new TypeError('callback is not function!');}
if(len===0&&arguments.length==2){throw new TypeError('arguments invalid');}
var k=0;var accumulator;if(arguments.length>=3){accumulator=arguments[2];}
else{do{if(k in arr){accumulator=arr[k++];break;}
k+=1;if(k>=len){throw new TypeError();}}
while(TRUE);}
while(k<len){if(k in arr){accumulator=callback.call(undefined,accumulator,arr[k],k,arr);}
k++;}
return accumulator;},every:every?function(arr,fn,context){return every.call(arr,fn,context||this);}:function(arr,fn,context){var len=arr&&arr.length||0;for(var i=0;i<len;i++){if(i in arr&&!fn.call(context,arr[i],i,arr)){return FALSE;}}
return TRUE;},some:some?function(arr,fn,context){return some.call(arr,fn,context||this);}:function(arr,fn,context){var len=arr&&arr.length||0;for(var i=0;i<len;i++){if(i in arr&&fn.call(context,arr[i],i,arr)){return TRUE;}}
return FALSE;},bind:function(fn,obj,arg1){var slice=[].slice,args=slice.call(arguments,2),fNOP=function(){},bound=function(){return fn.apply(this instanceof fNOP?this:obj,args.concat(slice.call(arguments)));};fNOP.prototype=fn.prototype;bound.prototype=new fNOP();return bound;},now:Date.now||function(){return+new Date();},fromUnicode:function(str){return str.replace(/\\u([a-f\d]{4})/ig,function(m,u){return String.fromCharCode(parseInt(u,HEX_BASE));});},ucfirst:function(s){s+='';return s.charAt(0).toUpperCase()+s.substring(1);},escapeHTML:function(str){return(str+'').replace(getEscapeReg(),function(m){return reverseEntities[m];});},escapeRegExp:function(str){return str.replace(escapeRegExp,'\\$&');},unEscapeHTML:function(str){return str.replace(getUnEscapeReg(),function(m,n){return htmlEntities[m]||String.fromCharCode(+n);});},makeArray:function(o){if(o==null){return[];}
if(S.isArray(o)){return o;}
if(typeof o.length!=='number'||o.alert||S.isString(o)||S.isFunction(o)){return[o];}
var ret=[];for(var i=0,l=o.length;i<l;i++){ret[i]=o[i];}
return ret;},param:function(o,sep,eq,serializeArray){if(!S.isPlainObject(o)){return EMPTY;}
sep=sep||SEP;eq=eq||EQ;if(S.isUndefined(serializeArray)){serializeArray=TRUE;}
var buf=[],key,i,v,len,val;for(key in o){if(o.hasOwnProperty(key)){val=o[key];key=encode(key);if(isValidParamValue(val)){buf.push(key);if(val!==undefined){buf.push(eq,encode(val+EMPTY));}
buf.push(sep);}
else if(S.isArray(val)&&val.length){for(i=0,len=val.length;i<len;++i){v=val[i];if(isValidParamValue(v)){buf.push(key,(serializeArray?encode('[]'):EMPTY));if(v!==undefined){buf.push(eq,encode(v+EMPTY));}
buf.push(sep);}}}}}
buf.pop();return buf.join(EMPTY);},unparam:function(str,sep,eq){if(!S.isString(str)||!(str=S.trim(str))){return{};}
sep=sep||SEP;eq=eq||EQ;var ret={},eqIndex,pairs=str.split(sep),key,val,i=0,len=pairs.length;for(;i<len;++i){eqIndex=pairs[i].indexOf(eq);if(eqIndex==-1){key=decode(pairs[i]);val=undefined;}else{key=decode(pairs[i].substring(0,eqIndex));val=pairs[i].substring(eqIndex+1);try{val=decode(val);}catch(e){S.log(e+'decodeURIComponent error : '+val,'error');}
if(S.endsWith(key,'[]')){key=key.substring(0,key.length-2);}}
if(hasOwnProperty(ret,key)){if(S.isArray(ret[key])){ret[key].push(val);}else{ret[key]=[ret[key],val];}}else{ret[key]=val;}}
return ret;},later:function(fn,when,periodic,context,data){when=when||0;var m=fn,d=S.makeArray(data),f,r;if(S.isString(fn)){m=context[fn];}
if(!m){S.error('method undefined');}
f=function(){m.apply(context,d);};r=(periodic)?setInterval(f,when):setTimeout(f,when);return{id:r,interval:periodic,cancel:function(){if(this.interval){clearInterval(r);}else{clearTimeout(r);}}};},startsWith:function(str,prefix){return str.lastIndexOf(prefix,0)===0;},endsWith:function(str,suffix){var ind=str.length-suffix.length;return ind>=0&&str.indexOf(suffix,ind)==ind;},throttle:function(fn,ms,context){ms=ms||150;if(ms===-1){return(function(){fn.apply(context||this,arguments);});}
var last=S.now();return(function(){var now=S.now();if(now-last>ms){last=now;fn.apply(context||this,arguments);}});},buffer:function(fn,ms,context){ms=ms||150;if(ms===-1){return function(){fn.apply(context||this,arguments);};}
var bufferTimer=null;function f(){f.stop();bufferTimer=S.later(fn,ms,FALSE,context||this,arguments);}
f.stop=function(){if(bufferTimer){bufferTimer.cancel();bufferTimer=0;}};return f;}});S.mix(S,{isBoolean:isValidParamValue,isNumber:isValidParamValue,isString:isValidParamValue,isFunction:isValidParamValue,isArray:isValidParamValue,isDate:isValidParamValue,isRegExp:isValidParamValue,isObject:isValidParamValue});S.each('Boolean Number String Function Array Date RegExp Object'.split(' '),function(name,lc){class2type['[object '+name+']']=(lc=name.toLowerCase());S['is'+name]=function(o){return S.type(o)==lc;}});function cloneInternal(input,f,memory){var destination=input,isArray,isPlainObject,k,stamp;if(!input){return destination;}
if(input[CLONE_MARKER]){return memory[input[CLONE_MARKER]].destination;}else if(typeof input==='object'){var constructor=input.constructor;if(S.inArray(constructor,[Boolean,String,Number,Date,RegExp])){destination=new constructor(input.valueOf());}
else if(isArray=S.isArray(input)){destination=f?S.filter(input,f):input.concat();}else if(isPlainObject=S.isPlainObject(input)){destination={};}
input[CLONE_MARKER]=(stamp=S.guid());memory[stamp]={destination:destination,input:input};}
if(isArray){for(var i=0;i<destination.length;i++){destination[i]=cloneInternal(destination[i],f,memory);}}else if(isPlainObject){for(k in input){if(input.hasOwnProperty(k)){if(k!==CLONE_MARKER&&(!f||(f.call(input,input[k],k,input)!==FALSE))){destination[k]=cloneInternal(input[k],f,memory);}}}}
return destination;}
function compareObjects(a,b,mismatchKeys,mismatchValues){if(a[COMPARE_MARKER]===b&&b[COMPARE_MARKER]===a){return TRUE;}
a[COMPARE_MARKER]=b;b[COMPARE_MARKER]=a;var hasKey=function(obj,keyName){return(obj!==null&&obj!==undefined)&&obj[keyName]!==undefined;};for(var property in b){if(b.hasOwnProperty(property)){if(!hasKey(a,property)&&hasKey(b,property)){mismatchKeys.push("expected has key '"+property+"', but missing from actual.");}}}
for(property in a){if(a.hasOwnProperty(property)){if(!hasKey(b,property)&&hasKey(a,property)){mismatchKeys.push("expected missing key '"+property+"', but present in actual.");}}}
for(property in b){if(b.hasOwnProperty(property)){if(property==COMPARE_MARKER){continue;}
if(!S.equals(a[property],b[property],mismatchKeys,mismatchValues)){mismatchValues.push("'"+property+"' was '"+(b[property]?(b[property].toString()):b[property])
+"' in expected, but was '"+
(a[property]?(a[property].toString()):a[property])+"' in actual.");}}}
if(S.isArray(a)&&S.isArray(b)&&a.length!=b.length){mismatchValues.push('arrays were not the same length');}
delete a[COMPARE_MARKER];delete b[COMPARE_MARKER];return(mismatchKeys.length===0&&mismatchValues.length===0);}})(KISSY);(function(S,undefined){function promiseWhen(promise,fulfilled,rejected){if(promise instanceof Reject){if(!rejected){S.error('no rejected callback!');}
return rejected(promise.__promise_value);}
var v=promise.__promise_value,pendings=promise.__promise_pendings;if(pendings){pendings.push([fulfilled,rejected]);}
else if(isPromise(v)){promiseWhen(v,fulfilled,rejected);}else{return fulfilled&&fulfilled(v);}
return undefined;}
function Defer(promise){var self=this;if(!(self instanceof Defer)){return new Defer(promise);}
self.promise=promise||new Promise();}
Defer.prototype={constructor:Defer,resolve:function(value){var promise=this.promise,pendings;if(!(pendings=promise.__promise_pendings)){return undefined;}
promise.__promise_value=value;pendings=[].concat(pendings);promise.__promise_pendings=undefined;S.each(pendings,function(p){promiseWhen(promise,p[0],p[1]);});return value;},reject:function(reason){return this.resolve(new Reject(reason));}};function isPromise(obj){return obj&&obj instanceof Promise;}
function Promise(v){var self=this;self.__promise_value=v;if(v===undefined){self.__promise_pendings=[];}}
Promise.prototype={constructor:Promise,then:function(fulfilled,rejected){return when(this,fulfilled,rejected);},fail:function(rejected){return when(this,0,rejected);},fin:function(callback){return when(this,function(value){return callback(value,true);},function(reason){return callback(reason,false);});},isResolved:function(){return isResolved(this);},isRejected:function(){return isRejected(this);}};function Reject(reason){if(reason instanceof Reject){return reason;}
var self=this;Promise.apply(self,arguments);if(self.__promise_value instanceof Promise){S.error('assert.not(this.__promise_value instanceof promise) in Reject constructor');}
return undefined;}
S.extend(Reject,Promise);function when(value,fulfilled,rejected){var defer=new Defer(),done=0;function _fulfilled(value){try{return fulfilled?fulfilled(value):value;}catch(e){S.log(e.stack||e,'error');return new Reject(e);}}
function _rejected(reason){try{return rejected?rejected(reason):new Reject(reason);}catch(e){S.log(e.stack||e,'error');return new Reject(e);}}
function finalFulfill(value){if(done){S.error('already done at fulfilled');return;}
if(value instanceof Promise){S.error('assert.not(value instanceof Promise) in when')}
done=1;defer.resolve(_fulfilled(value));}
if(value instanceof Promise){promiseWhen(value,finalFulfill,function(reason){if(done){S.error('already done at rejected');return;}
done=1;defer.resolve(_rejected(reason));});}else{finalFulfill(value);}
return defer.promise;}
function isResolved(obj){return!isRejected(obj)&&isPromise(obj)&&(obj.__promise_pendings===undefined)&&(!isPromise(obj.__promise_value)||isResolved(obj.__promise_value));}
function isRejected(obj){return isPromise(obj)&&(obj.__promise_pendings===undefined)&&(obj.__promise_value instanceof Reject);}
KISSY.Defer=Defer;KISSY.Promise=Promise;S.mix(Promise,{when:when,isPromise:isPromise,isResolved:isResolved,isRejected:isRejected,all:function(promises){var count=promises.length;if(!count){return promises;}
var defer=Defer();for(var i=0;i<promises.length;i++){(function(promise,i){when(promise,function(value){promises[i]=value;if(--count===0){defer.resolve(promises);}},function(r){defer.reject(r);});})(promises[i],i);}
return defer.promise;}});})(KISSY);(function(S){var splitPathRe=/^(\/?)([\s\S]+\/(?!$)|\/)?((?:\.{1,2}$|[\s\S]+?)?(\.[^.\/]*)?)$/;function normalizeArray(parts,allowAboveRoot){var up=0;for(var i=parts.length-1;i>=0;i--){var last=parts[i];if(last=='.'){parts.splice(i,1);}else if(last==='..'){parts.splice(i,1);up++;}else if(up){parts.splice(i,1);up--;}}
if(allowAboveRoot){for(;up--;up){parts.unshift('..');}}
return parts;}
var Path={resolve:function(){var resolvedPath='',resolvedPathStr,i,args=S.makeArray(arguments),path,absolute=0;for(i=args.length-1;i>=0&&!absolute;i--){path=args[i];if(typeof path!='string'||!path){continue;}
resolvedPath=path+'/'+resolvedPath;absolute=path.charAt(0)=='/';}
resolvedPathStr=normalizeArray(S.filter(resolvedPath.split('/'),function(p){return!!p;}),!absolute).join('/');return((absolute?'/':'')+resolvedPathStr)||'.';},normalize:function(path){var absolute=path.charAt(0)=='/',trailingSlash=path.slice(-1)=='/';path=normalizeArray(S.filter(path.split('/'),function(p){return!!p;}),!absolute).join('/');if(!path&&!absolute){path='.';}
if(path&&trailingSlash){path+='/';}
return(absolute?'/':'')+path;},join:function(){var args=S.makeArray(arguments);return Path.normalize(S.filter(args,function(p){return p&&(typeof p=='string');}).join('/'));},relative:function(from,to){from=Path.normalize(from);to=Path.normalize(to);var fromParts=S.filter(from.split('/'),function(p){return!!p;}),path=[],sameIndex,sameIndex2,toParts=S.filter(to.split('/'),function(p){return!!p;}),commonLength=Math.min(fromParts.length,toParts.length);for(sameIndex=0;sameIndex<commonLength;sameIndex++){if(fromParts[sameIndex]!=toParts[sameIndex]){break;}}
sameIndex2=sameIndex;while(sameIndex<fromParts.length){path.push('..');sameIndex++;}
path=path.concat(toParts.slice(sameIndex2));path=path.join('/');return path;},basename:function(path,ext){var result=path.match(splitPathRe)||[];result=result[3]||'';if(ext&&result&&result.slice(-1*ext.length)==ext){result=result.slice(0,-1*ext.length);}
return result;},dirname:function(path){var result=path.match(splitPathRe)||[],root=result[1]||'',dir=result[2]||'';if(!root&&!dir){return'.';}
if(dir){dir=dir.substring(0,dir.length-1);}
return root+dir;},extname:function(path){return(path.match(splitPathRe)||[])[4]||'';}};S.Path=Path;})(KISSY);(function(S,undefined){var reDisallowedInSchemeOrUserInfo=/[#\/\?@]/g,reDisallowedInPathName=/[#\?]/g,reDisallowedInQuery=/[#@]/g,reDisallowedInFragment=/#/g,URI_SPLIT_REG=new RegExp('^'+'(?:([\\w\\d+.-]+):)?'+'(?://'+'(?:([^/?#@]*)@)?'+'('+'[\\w\\d\\-\\u0100-\\uffff.+%]*'+'|'+'\\[[^\\]]+\\]'+')'+'(?::([0-9]+))?'+')?'+'([^?#]+)?'+'(?:\\?([^#]*))?'+'(?:#(.*))?'+'$'),Path=S.Path,REG_INFO={scheme:1,userInfo:2,hostname:3,port:4,path:5,query:6,fragment:7};function parseQuery(self){if(!self._queryMap){self._queryMap=S.unparam(self._query);}}
function Query(query){this._query=query||'';}
Query.prototype={constructor:Query,clone:function(){return new Query(this.toString());},reset:function(query){var self=this;self._query=query||'';self._queryMap=0;},count:function(){var self=this,count=0,_queryMap=self._queryMap,k;parseQuery(self);for(k in _queryMap){if(_queryMap.hasOwnProperty(k)){if(S.isArray(_queryMap[k])){count+=_queryMap[k].length;}else{count++;}}}
return count;},get:function(key){var self=this;parseQuery(self);if(key){return self._queryMap[key];}else{return self._queryMap;}},keys:function(){var self=this;parseQuery(self);return S.keys(self._queryMap);},set:function(key,value){var self=this,_queryMap;parseQuery(self);_queryMap=self._queryMap;if(S.isString(key)){self._queryMap[key]=value;}else{if(key instanceof Query){key=key.get();}
S.each(key,function(v,k){_queryMap[k]=v;});}
return self;},remove:function(key){var self=this;parseQuery(self);if(key){delete self._queryMap[key];}else{self._queryMap={};}
return self;},add:function(key,value){var self=this,_queryMap,currentValue;if(S.isObject(key)){if(key instanceof Query){key=key.get();}
S.each(key,function(v,k){self.add(k,v);});}else{parseQuery(self);_queryMap=self._queryMap;currentValue=_queryMap[key];if(currentValue===undefined){currentValue=value;}else{currentValue=[].concat(currentValue).concat(value);}
_queryMap[key]=currentValue;}
return self;},toString:function(serializeArray){var self=this;parseQuery(self);return S.param(self._queryMap,undefined,undefined,serializeArray);}};function padding2(str){return str.length==1?'0'+str:str;}
function equalsIgnoreCase(str1,str2){return str1.toLowerCase()==str2.toLowerCase();}
function encodeSpecialChars(str,specialCharsReg){return encodeURI(str).replace(specialCharsReg,function(m){return'%'+padding2(m.charCodeAt(0).toString(16));});}
function Uri(uriStr){if(uriStr instanceof Uri){return uriStr.clone();}
var m,self=this;S.mix(self,{scheme:'',userInfo:'',hostname:'',port:'',path:'',query:'',fragment:''});uriStr=uriStr||'';m=uriStr.match(URI_SPLIT_REG)||[];S.each(REG_INFO,function(index,key){var match=m[index]||'';if(key=='query'){self.query=new Query(match);}else{self[key]=decodeURIComponent(match);}});}
Uri.prototype={constructor:Uri,clone:function(){var uri=new Uri(),self=this;S.each(REG_INFO,function(index,key){uri[key]=self[key];});uri.query=uri.query.clone();return uri;},resolve:function(relativeUri){if(S.isString(relativeUri)){relativeUri=new Uri(relativeUri);}
var self=this,override=0,lastSlashIndex,order=['scheme','userInfo','hostname','port','path','query','fragment'],target=self.clone();S.each(order,function(o){if(o=='path'){if(override){target[o]=relativeUri[o];}else{var path=relativeUri['path'];if(path){override=1;if(!S.startsWith(path,'/')){if(target.hostname&&!target.path){path='/'+path;}else if(target.path){lastSlashIndex=target.path.lastIndexOf('/');if(lastSlashIndex!=-1){path=target.path.slice(0,lastSlashIndex+1)+path;}}}
target.path=Path.normalize(path);}}}else if(o=='query'){if(override||relativeUri['query'].toString()){target.query=relativeUri['query'].clone();override=1;}}else if(override||relativeUri[o]){target[o]=relativeUri[o];override=1;}});return target;},getScheme:function(){return this.scheme;},setScheme:function(scheme){this.scheme=scheme;return this;},getHostname:function(){return this.hostname;},setHostname:function(hostname){this.hostname=hostname;return this;},setUserInfo:function(userInfo){this.userInfo=userInfo;return this;},getUserInfo:function(){return this.userInfo;},setPort:function(port){this.port=port;return this;},getPort:function(){return this.port;},setPath:function(path){this.path=path;return this;},getPath:function(){return this.path;},setQuery:function(query){if(S.isString(query)){if(S.startsWith(query,'?')){query=query.slice(1);}
query=new Query(encodeSpecialChars(query,reDisallowedInQuery));}
this.query=query;return this;},getQuery:function(){return this.query;},getFragment:function(){return this.fragment;},setFragment:function(fragment){if(!S.startsWith(fragment,'#')){fragment='#'+fragment;}
this.fragment=fragment;return this;},hasSameDomainAs:function(other){var self=this;return equalsIgnoreCase(self.hostname,other['hostname'])&&equalsIgnoreCase(self.scheme,other['scheme'])&&equalsIgnoreCase(self.port,other['port']);},toString:function(serializeArray){var out=[],self=this,scheme,hostname,path,port,fragment,query,userInfo;if(scheme=self.scheme){out.push(encodeSpecialChars(scheme,reDisallowedInSchemeOrUserInfo));out.push(':');}
if(hostname=self.hostname){out.push('//');if(userInfo=self.userInfo){out.push(encodeSpecialChars(userInfo,reDisallowedInSchemeOrUserInfo));out.push('@');}
out.push(encodeURIComponent(hostname));if(port=self.port){out.push(':');out.push(port);}}
if(path=self.path){if(hostname&&!S.startsWith(path,'/')){path='/'+path;}
path=Path.normalize(path);out.push(encodeSpecialChars(path,reDisallowedInPathName));}
if(query=(self.query.toString(serializeArray))){out.push('?');out.push(query);}
if(fragment=self.fragment){out.push('#');out.push(encodeSpecialChars(fragment,reDisallowedInFragment))}
return out.join('');}};Uri.Query=Query;S.Uri=Uri;})(KISSY);(function(S){if(S.Env.nodejs){return;}
var Path=S.Path;function Loader(SS){this.SS=SS;}
KISSY.Loader=Loader;function Package(cfg){S.mix(this,cfg);}
S.augment(Package,{getTag:function(){var self=this;return self.tag||self.SS.Config.tag;},getName:function(){return this.name;},getBase:function(){var self=this;return self.base||self.SS.Config.base;},getBaseUri:function(){var self=this;return self.baseUri||self.SS.Config.baseUri;},isDebug:function(){var self=this,debug=self.debug;return debug===undefined?self.SS.Config.debug:debug;},getCharset:function(){var self=this;return self.charset||self.SS.Config.charset;},isCombine:function(){var self=this,combine=self.combine;return combine===undefined?self.SS.Config.combine:combine;}});Loader.Package=Package;function Module(cfg){S.mix(this,cfg);}
S.augment(Module,{setValue:function(v){this.value=v;},getType:function(){var self=this,v;if((v=self.type)===undefined){if(Path.extname(self.name).toLowerCase()=='.css'){v='css';}else{v='js';}
self.type=v;}
return v;},getFullPath:function(){var self=this,t,fullpathUri,packageBaseUri;if(!self.fullpath){packageBaseUri=self.getPackageInfo().getBaseUri();fullpathUri=packageBaseUri.resolve(self.getPath());if(t=self.getTag()){fullpathUri.query.set('t',t);}
self.fullpath=Loader.Utils.getMappedPath(self.SS,fullpathUri.toString());}
return self.fullpath;},getPath:function(){var self=this;return self.path||(self.path=defaultComponentJsName(self))},getValue:function(){return this.value;},getName:function(){return this.name;},getPackageInfo:function(){var self=this;return self.packageInfo||(self.packageInfo=getPackageInfo(self.SS,self));},getTag:function(){var self=this;return self.tag||self.getPackageInfo().getTag();},getCharset:function(){var self=this;return self.charset||self.getPackageInfo().getCharset();}});Loader.Module=Module;function defaultComponentJsName(m){var name=m.name,extname=(Path.extname(name)||'').toLowerCase(),min='-min';if(extname!='.css'){extname='.js';}
name=Path.join(Path.dirname(name),Path.basename(name,extname));if(m.getPackageInfo().isDebug()){min='';}
return name+min+extname;}
function getPackageInfo(self,mod){var modName=mod.name,Env=self.Env,packages=Env.packages||{},pName='',p,packageDesc;for(p in packages){if(packages.hasOwnProperty(p)){if(S.startsWith(modName,p)&&p.length>pName.length){pName=p;}}}
packageDesc=packages[pName]||Env.defaultPackage||(Env.defaultPackage=new Loader.Package({SS:self,name:''}));return packageDesc;}
Loader.STATUS={'INIT':0,'LOADING':1,'LOADED':2,'ERROR':3,'ATTACHED':4};})(KISSY);(function(S){S.namespace("Loader");var time=S.now(),p='__events__'+time;function getHolder(self){return self[p]||(self[p]={});}
function getEventHolder(self,name,create){var holder=getHolder(self);if(create){holder[name]=holder[name]||[];}
return holder[name];}
KISSY.Loader.Target={on:function(eventName,callback){getEventHolder(this,eventName,1).push(callback);},detach:function(eventName,callback){var self=this,fns,index;if(!eventName){delete self[p];return;}
fns=getEventHolder(self,eventName);if(fns){if(callback){index=S.indexOf(callback,fns);if(index!=-1){fns.splice(index,1);}}
if(!callback||!fns.length){delete getHolder(self)[eventName];}}},fire:function(eventName,obj){var fns=getEventHolder(this,eventName);S.each(fns,function(f){f.call(null,obj);});}};})(KISSY);(function(S){if(S.Env.nodejs){return;}
var Loader=S.Loader,Path=S.Path,Uri=S.Uri,ua=navigator.userAgent,startsWith=S.startsWith,data=Loader.STATUS,Utils={},host=S.Env.host,isWebKit=!!ua.match(/AppleWebKit/),doc=host.document,simulatedLocation=new Uri(location.href);function indexMap(s){if(S.isArray(s)){var ret=[],i=0;for(;i<s.length;i++){ret[i]=indexMapStr(s[i]);}
return ret;}
return indexMapStr(s);}
function indexMapStr(s){if(S.endsWith(Path.basename(s),'/')){s+='index';}
return s;}
S.mix(Utils,{docHead:function(){return doc.getElementsByTagName('head')[0]||doc.documentElement;},isWebKit:isWebKit,isGecko:!isWebKit&&!!ua.match(/Gecko/),isPresto:!!ua.match(/Presto/),IE:!!ua.match(/MSIE/),normalDepModuleName:function(moduleName,depName){var i=0;if(!depName){return depName;}
if(S.isArray(depName)){for(;i<depName.length;i++){depName[i]=Utils.normalDepModuleName(moduleName,depName[i]);}
return depName;}
if(startsWith(depName,'../')||startsWith(depName,'./')){return Path.resolve(Path.dirname(moduleName),depName);}
return Path.normalize(depName);},removeExtname:function(path){return path.replace(/(-min)?\.js$/i,'');},resolveByPage:function(path){return simulatedLocation.resolve(path);},createModulesInfo:function(self,modNames){S.each(modNames,function(m){Utils.createModuleInfo(self,m);});},createModuleInfo:function(self,modName,cfg){modName=indexMapStr(modName);var mods=self.Env.mods,mod=mods[modName];if(mod){return mod;}
mods[modName]=mod=new Loader.Module(S.mix({name:modName,SS:self},cfg));return mod;},isAttached:function(self,modNames){return isStatus(self,modNames,data.ATTACHED);},isLoaded:function(self,modNames){return isStatus(self,modNames,data.LOADED);},getModules:function(self,modNames){var mods=[self],mod;S.each(modNames,function(modName){mod=self.Env.mods[modName];if(!mod||mod.getType()!='css'){mods.push(self.require(modName));}});return mods;},attachMod:function(self,mod){if(mod.status!=data.LOADED){return;}
var fn=mod.fn,requires,value;requires=mod.requires=Utils.normalizeModNamesWithAlias(self,mod.requires,mod.name);if(fn){if(S.isFunction(fn)){value=fn.apply(mod,Utils.getModules(self,requires));}else{value=fn;}
mod.value=value;}
mod.status=data.ATTACHED;self.getLoader().fire('afterModAttached',{mod:mod});},getModNamesAsArray:function(modNames){if(S.isString(modNames)){modNames=modNames.replace(/\s+/g,'').split(',');}
return modNames;},normalizeModNames:function(self,modNames,refModName){return Utils.unalias(self,Utils.normalizeModNamesWithAlias(self,modNames,refModName));},unalias:function(self,names){var ret=[].concat(names),i,m,alias,ok=0,mods=self['Env'].mods;while(!ok){ok=1;for(i=ret.length-1;i>=0;i--){if((m=mods[ret[i]])&&(alias=m.alias)){ok=0;ret.splice.apply(ret,[i,1].concat(indexMap(alias)));}}}
return ret;},normalizeModNamesWithAlias:function(self,modNames,refModName){var ret=[],i,l;if(modNames){for(i=0,l=modNames.length;i<l;i++){if(modNames[i]){ret.push(indexMap(modNames[i]));}}}
if(refModName){ret=Utils.normalDepModuleName(refModName,ret);}
return ret;},registerModule:function(self,name,fn,config){var mods=self.Env.mods,mod=mods[name];if(mod&&mod.fn){S.log(name+' is defined more than once');return;}
Utils.createModuleInfo(self,name);mod=mods[name];S.mix(mod,{name:name,status:data.LOADED});mod.fn=fn;S.mix((mods[name]=mod),config);S.log(name+' is loaded');},getMappedPath:function(self,path,rules){var __mappedRules=rules||self.Config.mappedRules||[],i,m,rule;for(i=0;i<__mappedRules.length;i++){rule=__mappedRules[i];if(m=path.match(rule[0])){return path.replace(rule[0],rule[1]);}}
return path;}});function isStatus(self,modNames,status){var mods=self.Env.mods,i;modNames=S.makeArray(modNames);for(i=0;i<modNames.length;i++){var mod=mods[modNames[i]];if(!mod||mod.status!==status){return false;}}
return true;}
Loader.Utils=Utils;})(KISSY);(function(S){if(S.Env.nodejs){return;}
var CSS_POLL_INTERVAL=30,win=S.Env.host,utils=S.Loader.Utils,timer=0,monitors={};function startCssTimer(){if(!timer){cssPoll();}}
function cssPoll(){for(var url in monitors){if(monitors.hasOwnProperty(url)){var callbackObj=monitors[url],node=callbackObj.node,exName,loaded=0;if(utils.isWebKit){if(node['sheet']){S.log('webkit loaded : '+url);loaded=1;}}else if(node['sheet']){try{var cssRules;if(cssRules=node['sheet'].cssRules){S.log('firefox loaded : '+url);loaded=1;}}catch(ex){exName=ex.name;S.log('firefox getStyle : '+exName+' '+ex.code+' '+url);if(exName=='SecurityError'||exName=='NS_ERROR_DOM_SECURITY_ERR'){S.log('firefox loaded : '+url);loaded=1;}}}
if(loaded){if(callbackObj.callback){callbackObj.callback.call(node);}
delete monitors[url];}}}
if(S.isEmptyObject(monitors)){timer=0;}else{timer=setTimeout(cssPoll,CSS_POLL_INTERVAL);}}
S.mix(utils,{styleOnLoad:win.attachEvent||utils.isPresto?function(node,callback){function t(){node.detachEvent('onload',t);S.log('ie/opera loaded : '+node.href);callback.call(node);}
node.attachEvent('onload',t);}:function(node,callback){var href=node.href,arr;arr=monitors[href]={};arr.node=node;arr.callback=callback;startCssTimer();}});})(KISSY);(function(S){if(S.Env.nodejs){return;}
var MILLISECONDS_OF_SECOND=1000,doc=S.Env.host.document,utils=S.Loader.Utils,Path=S.Path,jsCallbacks={},cssCallbacks={};S.mix(S,{getStyle:function(url,success,charset){var config=success;if(S.isPlainObject(config)){success=config.success;charset=config.charset;}
var src=utils.resolveByPage(url).toString(),callbacks=cssCallbacks[src]=cssCallbacks[src]||[];callbacks.push(success);if(callbacks.length>1){return callbacks.node;}
var head=utils.docHead(),node=doc.createElement('link');callbacks.node=node;node.href=url;node.rel='stylesheet';if(charset){node.charset=charset;}
utils.styleOnLoad(node,function(){var callbacks=cssCallbacks[src];S.each(callbacks,function(callback){if(callback){callback.call(node);}});delete cssCallbacks[src];});head.appendChild(node);return node;},getScript:function(url,success,charset){if(S.startsWith(Path.extname(url).toLowerCase(),'.css')){return S.getStyle(url,success,charset);}
var src=utils.resolveByPage(url),config=success,error,timeout,timer;if(S.isPlainObject(config)){success=config.success;error=config.error;timeout=config.timeout;charset=config.charset;}
var callbacks=jsCallbacks[src]=jsCallbacks[src]||[];callbacks.push([success,error]);if(callbacks.length>1){return callbacks.node;}else{}
var head=utils.docHead(),node=doc.createElement('script'),clearTimer=function(){if(timer){timer.cancel();timer=undefined;}};node.src=url;node.async=true;callbacks.node=node;if(charset){node.charset=charset;}
var end=function(error){var index=error?1:0;clearTimer();var callbacks=jsCallbacks[src];S.each(callbacks,function(callback){if(callback[index]){callback[index].call(node);}});delete jsCallbacks[src];};if(node.addEventListener){node.addEventListener('load',function(){end(0);},false);node.addEventListener('error',function(){end(1);},false);}else{node.onreadystatechange=function(){var self=this,rs=self.readyState;if(/loaded|complete/i.test(rs)){self.onreadystatechange=null;end(0);}};}
if(timeout){timer=S.later(function(){end(1);},timeout*MILLISECONDS_OF_SECOND);}
head.insertBefore(node,head.firstChild);return node;}});})(KISSY);(function(S){if(S.Env.nodejs){return;}
var Loader=S.Loader,utils=Loader.Utils,configs=S.configs;configs.map=function(rules){var self=this;return self.Config.mappedRules=(self.Config.mappedRules||[]).concat(rules||[]);};configs.mapCombo=function(rules){var self=this;return self.Config.mappedComboRules=(self.Config.mappedComboRules||[]).concat(rules||[]);};configs.packages=function(cfgs){var self=this,name,base,Env=self.Env,ps=Env.packages=Env.packages||{};if(cfgs){S.each(cfgs,function(cfg,key){name=cfg.name||key;base=cfg.base||cfg.path;if(!S.endsWith(base,'/')){base+='/';}
cfg.name=name;var baseUri=utils.resolveByPage(base);cfg.base=baseUri.toString();cfg.baseUri=baseUri;cfg.SS=S;delete cfg.path;ps[name]=new Loader.Package(cfg);});}};configs.modules=function(modules){var self=this;if(modules){S.each(modules,function(modCfg,modName){utils.createModuleInfo(self,modName,modCfg);S.mix(self.Env.mods[modName],modCfg);});}};configs.base=function(base){var self=this,baseUri,Config=self.Config;if(!base){return Config.base;}
baseUri=utils.resolveByPage(base);Config.base=baseUri.toString();Config.baseUri=baseUri;};})(KISSY);(function(S,undefined){if(S.Env.nodejs){return;}
var Loader=S.Loader,Path=S.Path,utils=Loader.Utils;S.augment(Loader,Loader.Target,{__currentModule:null,__startLoadTime:0,__startLoadModuleName:null,add:function(name,fn,config){var self=this,SS=self.SS,mod,requires,mods=SS.Env.mods;if(S.isPlainObject(name)){return SS.config({modules:name});}
if(S.isString(name)){utils.registerModule(SS,name,fn,config);mod=mods[name];if(config&&config['attach']===false){return;}
if(config){requires=utils.normalizeModNames(SS,config.requires,name);}
if(!requires||utils.isAttached(SS,requires)){utils.attachMod(SS,mod);}
return;}
else if(S.isFunction(name)){config=fn;fn=name;if(utils.IE){name=findModuleNameByInteractive(self);S.log('old_ie get modName by interactive : '+name);utils.registerModule(SS,name,fn,config);self.__startLoadModuleName=null;self.__startLoadTime=0;}else{self.__currentModule={fn:fn,config:config};}
return;}
S.log('invalid format for KISSY.add !','error');}});function findModuleNameByInteractive(self){var SS=self.SS,scripts=S.Env.host.document.getElementsByTagName('script'),re,i,script;for(i=0;i<scripts.length;i++){script=scripts[i];if(script.readyState=='interactive'){re=script;break;}}
if(!re){S.log('can not find interactive script,time diff : '+(+new Date()-self.__startLoadTime),'error');S.log('old_ie get mod name from cache : '+self.__startLoadModuleName);return self.__startLoadModuleName;}
var src=utils.resolveByPage(re.src),srcStr=src.toString(),packages=SS.Env.packages,finalPackagePath,p,packageBase,Config=SS.Config,finalPackageUri,finalPackageLength=-1;for(p in packages){if(packages.hasOwnProperty(p)){packageBase=packages[p].getBase();if(S.startsWith(srcStr,packageBase)){if(packageBase.length>finalPackageLength){finalPackageLength=packageBase.length;finalPackagePath=packageBase;finalPackageUri=packages[p].getBaseUri();}}}}
if(finalPackagePath){return utils.removeExtname(Path.relative(finalPackageUri.getPath(),src.getPath()));}else if(S.startsWith(srcStr,Config.base)){return utils.removeExtname(Path.relative(Config.baseUri.getPath(),src.getPath()));}
S.log('interactive script does not have package config £º'+src,'error');return undefined;}})(KISSY);(function(S){if(S.Env.nodejs){return;}
var Loader=S.Loader,data=Loader.STATUS,utils=Loader.Utils,INIT=data.INIT,IE=utils.IE,win=S.Env.host,LOADING=data.LOADING,ERROR=data.ERROR,ALL_REQUIRES='__allRequires',CURRENT_MODULE='__currentModule',ATTACHED=data.ATTACHED;S.augment(Loader,{use:function(modNames,callback){var self=this,SS=self.SS;modNames=utils.getModNamesAsArray(modNames);modNames=utils.normalizeModNamesWithAlias(SS,modNames);var normalizedModNames=utils.unalias(SS,modNames),count=normalizedModNames.length,currentIndex=0;function end(){var mods=utils.getModules(SS,modNames);callback&&callback.apply(SS,mods);}
if(utils.isAttached(SS,normalizedModNames)){return end();}
S.each(normalizedModNames,function(modName){attachModByName(self,modName,function(){if((++currentIndex)==count){end();}});});return self;}});function attachModByName(self,modName,callback){var SS=self.SS,mod;utils.createModuleInfo(SS,modName);mod=SS.Env.mods[modName];if(mod.status===ATTACHED){callback();return;}
attachModRecursive(self,mod,callback);}
function attachModRecursive(self,mod,callback){var SS=self.SS,r,rMod,i,callbackBeCalled=0,newRequires,mods=SS.Env.mods;var requires=utils.normalizeModNames(SS,mod.requires,mod.name);function cyclicCheck(){var __allRequires=mod[ALL_REQUIRES]=mod[ALL_REQUIRES]||{},myName=mod.name,rMod,r__allRequires;S.each(requires,function(r){rMod=mods[r];__allRequires[r]=1;if(rMod&&(r__allRequires=rMod[ALL_REQUIRES])){S.mix(__allRequires,r__allRequires);}});if(__allRequires[myName]){S.log(__allRequires,'error');var JSON=win.JSON,error='';if(JSON){error=JSON.stringify(__allRequires);}
S.error('find cyclic dependency by mod '+myName+' between mods: '+error);}}
S.log(cyclicCheck());for(i=0;i<requires.length;i++){r=requires[i];rMod=mods[r];if(rMod&&rMod.status===ATTACHED){}else{attachModByName(self,r,fn);}}
loadModByScript(self,mod,function(){newRequires=utils.normalizeModNames(SS,mod.requires,mod.name);var needToLoad=[];for(i=0;i<newRequires.length;i++){var r=newRequires[i],rMod=mods[r],inA=S.inArray(r,requires);if(rMod&&rMod.status===ATTACHED||inA){}else{needToLoad.push(r);}}
if(needToLoad.length){for(i=0;i<needToLoad.length;i++){attachModByName(self,needToLoad[i],fn);}}else{fn();}});function fn(){if(newRequires&&!callbackBeCalled&&utils.isAttached(SS,newRequires)){utils.attachMod(SS,mod);if(mod.status==ATTACHED){callbackBeCalled=1;callback();}}}}
function loadModByScript(self,mod,callback){var SS=self.SS,modName=mod.getName(),charset=mod.getCharset(),url=mod.getFullPath(),isCss=mod.getType()=='css';mod.status=mod.status||INIT;if(mod.status<LOADING){mod.status=LOADING;if(IE&&!isCss){self.__startLoadModuleName=modName;self.__startLoadTime=Number(+new Date());}
S.getScript(url,{success:function(){if(isCss){utils.registerModule(SS,modName,S.noop);}else{var currentModule;if(currentModule=self[CURRENT_MODULE]){S.log('standard browser get mod name after load : '+modName);utils.registerModule(SS,modName,currentModule.fn,currentModule.config);self[CURRENT_MODULE]=null;}}
checkAndHandle();},error:checkAndHandle,charset:charset});}
else if(mod.status==LOADING){S.getScript(url,{success:checkAndHandle,charset:charset});}
else{checkAndHandle();}
function checkAndHandle(){if(mod.fn){callback();}else{_modError();}}
function _modError(){S.log(modName+' is not loaded! can not find module in path : '+url,'error');mod.status=ERROR;}}})(KISSY);(function(S){if(S.Env.nodejs){return;}
function loadScripts(urls,callback,charset){var count=urls&&urls.length;if(!count){callback();return;}
S.each(urls,function(url){S.getScript(url,function(){if(!(--count)){callback();}},charset||'utf-8');});}
var Loader=S.Loader,data=Loader.STATUS,utils=Loader.Utils;function ComboLoader(SS){S.mix(this,{SS:SS,queue:[],loading:0});}
S.augment(ComboLoader,Loader.Target);function next(self){var args;if(self.queue.length){args=self.queue.shift();_use(self,args.modNames,args.fn);}}
function enqueue(self,modNames,fn){self.queue.push({modNames:modNames,fn:fn});}
function _use(self,modNames,fn){var unaliasModNames,allModNames,comboUrls,css,countCss,p,SS=self.SS;self.loading=1;modNames=utils.getModNamesAsArray(modNames);modNames=utils.normalizeModNamesWithAlias(SS,modNames);unaliasModNames=utils.unalias(SS,modNames);allModNames=self.calculate(unaliasModNames);utils.createModulesInfo(SS,allModNames);comboUrls=self.getComboUrls(allModNames);css=comboUrls.css;countCss=0;for(p in css){countCss++;}
var jsOk=0,cssOk=!countCss;for(p in css){if(css.hasOwnProperty(p)){loadScripts(css[p],function(){if(!(--countCss)){for(var p in css){if(css.hasOwnProperty(p)){S.each(css[p].mods,function(m){utils.registerModule(SS,m.name,S.noop);});}}
cssOk=1;check(jsOk);}},css[p].charset);}}
function check(paramJsOk){jsOk=paramJsOk;if(cssOk&&jsOk){attachMods(self,unaliasModNames);if(utils.isAttached(SS,unaliasModNames)){fn.apply(null,utils.getModules(SS,modNames))}else{_use(self,modNames,fn)}}}
_useJs(comboUrls,check);}
function _useJs(comboUrls,check){var p,success,jss=comboUrls.js,countJss=0;for(p in jss){countJss++;}
if(!countJss){check(1);return;}
success=1;for(p in jss){if(jss.hasOwnProperty(p)){(function(p){loadScripts(jss[p],function(){var mods=jss[p].mods,mod,i;for(i=0;i<mods.length;i++){mod=mods[i];if(!mod.fn){S.log(mod.name+' is not loaded! can not find module in path : '+jss[p],'error');mod.status=data.ERROR;success=0;return;}}
if(success&&!(--countJss)){check(1);}},jss[p].charset);})(p);}}}
function attachMods(self,modNames){S.each(modNames,function(modName){attachMod(self,modName);});}
function attachMod(self,modName){var SS=self.SS,i,len,requires,r,mod=getModInfo(self,modName);if(!mod||utils.isAttached(SS,modName)){return undefined;}
requires=utils.normalizeModNames(SS,mod.requires,modName);len=requires.length;for(i=0;i<len;i++){r=requires[i];attachMod(self,r);if(!utils.isAttached(SS,r)){return false;}}
utils.attachMod(SS,mod);return undefined;}
function getModInfo(self,modName){return self.SS.Env.mods[modName];}
function getRequires(self,modName,cache){var SS=self.SS,requires,i,rMod,r,allRequires,ret2,mod=getModInfo(self,modName),ret=cache[modName];if(ret){return ret;}
cache[modName]=ret={};if(mod&&!utils.isAttached(SS,modName)){requires=utils.normalizeModNames(SS,mod.requires,modName);if(S.Config.debug){allRequires=mod.__allRequires||(mod.__allRequires={});if(allRequires[modName]){S.error('detect circular dependency among : ');S.error(allRequires);return ret;}}
for(i=0;i<requires.length;i++){r=requires[i];if(S.Config.debug){rMod=getModInfo(self,r);allRequires[r]=1;if(rMod&&rMod.__allRequires){S.each(rMod.__allRequires,function(_,r2){allRequires[r2]=1;});}}
if(!utils.isLoaded(SS,r)&&!utils.isAttached(SS,r)){ret[r]=1;}
ret2=getRequires(self,r,cache);S.mix(ret,ret2);}}
return ret;}
S.augment(ComboLoader,{use:function(modNames,callback){var self=this,fn=function(){if(callback){callback.apply(this,arguments);}
self.loading=0;next(self);};enqueue(self,modNames,fn);if(!self.loading){next(self);}},add:function(name,fn,config){var self=this,SS=self.SS;if(S.isPlainObject(name)){return SS.config({modules:name});}
utils.registerModule(SS,name,fn,config);},calculate:function(modNames){var ret={},i,m,r,ret2,self=this,SS=self.SS,cache={};for(i=0;i<modNames.length;i++){m=modNames[i];if(!utils.isAttached(SS,m)){if(!utils.isLoaded(SS,m)){ret[m]=1;}
S.mix(ret,getRequires(self,m,cache));}}
ret2=[];for(r in ret){if(ret.hasOwnProperty(r)){ret2.push(r);}}
return ret2;},getComboUrls:function(modNames){var self=this,i,SS=self.SS,Config=SS.Config,combos={};S.each(modNames,function(modName){var mod=getModInfo(self,modName),packageInfo=mod.getPackageInfo(),packageBase=packageInfo.getBase(),type=mod.getType(),mods,packageName=packageInfo.getName();combos[packageName]=combos[packageName]||{};if(!(mods=combos[packageName][type])){mods=combos[packageName][type]=combos[packageName][type]||[];mods.combine=1;if(packageInfo.isCombine()===false){mods.combine=0;}
mods.tag=packageInfo.getTag();mods.charset=packageInfo.getCharset();mods.packageBase=packageBase;}
mods.push(mod);});var res={js:{},css:{}},t,packageName,type,comboPrefix=Config.comboPrefix,comboSep=Config.comboSep,maxFileNum=Config.comboMaxFileNum,maxUrlLength=Config.comboMaxUrlLength;for(packageName in combos){for(type in combos[packageName]){t=[];var jss=combos[packageName][type],tag=jss.tag,suffix=(tag?('?t='+encodeURIComponent(tag)):''),suffixLength=suffix.length,packageBase=jss.packageBase,prefix,path,fullpath,l,packagePath=packageBase+
(packageName?(packageName+'/'):'');res[type][packageName]=[];res[type][packageName].charset=jss.charset;res[type][packageName].mods=[];prefix=packagePath+comboPrefix;l=prefix.length;function pushComboUrl(){res[type][packageName].push(utils.getMappedPath(SS,prefix+
t.join(comboSep)+
suffix,Config.mappedComboRules||[]));}
for(i=0;i<jss.length;i++){fullpath=jss[i].getFullPath();res[type][packageName].mods.push(jss[i]);if(!jss.combine||!S.startsWith(fullpath,packagePath)){res[type][packageName].push(fullpath);continue;}
path=fullpath.slice(packagePath.length).replace(/\?.*$/,'');t.push(path);if((t.length>maxFileNum)||(l+t.join(comboSep).length+suffixLength>maxUrlLength)){t.pop();pushComboUrl();t=[];i--;}}
if(t.length){pushComboUrl();}}}
return res;}});Loader.Combo=ComboLoader;})(KISSY);(function(S){if(S.Env.nodejs){return;}
var Loader=S.Loader,utils=Loader.Utils,ComboLoader=S.Loader.Combo;S.mix(S,{add:function(name,fn,cfg){this.getLoader().add(name,fn,cfg);},use:function(names,callback){this.getLoader().use(names,callback);},getLoader:function(){var self=this,env=self.Env;if(self.Config.combine){return env._comboLoader;}else{return env._loader;}},require:function(moduleName){var self=this,mods=self.Env.mods,mod=mods[moduleName];return mod&&mod.value;}});function returnJson(s){return(new Function('return '+s))();}
function getBaseInfo(){var baseReg=/^(.*)(seed|kissy)(?:-min)?\.js[^/]*/i,baseTestReg=/(seed|kissy)(?:-min)?\.js/i,comboPrefix,comboSep,scripts=S.Env.host.document.getElementsByTagName('script'),script=scripts[scripts.length-1],src=utils.resolveByPage(script.src).toString(),baseInfo=script.getAttribute('data-config');if(baseInfo){baseInfo=returnJson(baseInfo);}else{baseInfo={};}
src=src.replace(/%3f/gi,'?').replace(/%2c/gi,',');comboPrefix=baseInfo.comboPrefix=baseInfo.comboPrefix||'??';comboSep=baseInfo.comboSep=baseInfo.comboSep||',';var parts,base,index=src.indexOf(comboPrefix);if(index==-1){base=src.replace(baseReg,'$1');}else{base=src.substring(0,index);parts=src.substring(index+comboPrefix.length).split(comboSep);S.each(parts,function(part){if(part.match(baseTestReg)){base+=part.replace(baseReg,'$1');return false;}});}
return S.mix({base:base,baseUri:new S.Uri(base)},baseInfo);}
S.config(S.mix({comboMaxUrlLength:2000,charset:'utf-8',comboMaxFileNum:40,tag:'20121109163450'},getBaseInfo()));(function(){var env=S.Env;env.mods=env.mods||{};env._loader=new Loader(S);env._comboLoader=new ComboLoader(S);})();S.add('empty',S.noop);})(KISSY);(function(S,undefined){var win=S.Env.host,doc=win['document'],docElem=doc.documentElement,location=win.location,navigator=win.navigator,EMPTY='',readyDefer=new S.Defer(),readyPromise=readyDefer.promise,POLL_RETIRES=500,POLL_INTERVAL=40,RE_IDSTR=/^#?([\w-]+)$/,RE_NOT_WHITE=/\S/;S.mix(S,{isWindow:function(o){return S.type(o)==='object'&&'setInterval'in o&&'document'in o&&o.document.nodeType==9;},parseXML:function(data){if(data.documentElement){return data;}
var xml;try{if(win['DOMParser']){xml=new DOMParser().parseFromString(data,'text/xml');}else{xml=new ActiveXObject('Microsoft.XMLDOM');xml.async='false';xml.loadXML(data);}}catch(e){S.log('parseXML error : ');S.log(e);xml=undefined;}
if(!xml||!xml.documentElement||xml.getElementsByTagName('parsererror').length){S.error('Invalid XML: '+data);}
return xml;},globalEval:function(data){if(data&&RE_NOT_WHITE.test(data)){(win.execScript||function(data){win['eval'].call(win,data);})(data);}},ready:function(fn){readyPromise.then(fn);return this;},available:function(id,fn){id=(id+EMPTY).match(RE_IDSTR)[1];if(!id||!S.isFunction(fn)){return;}
var retryCount=1,node,timer=S.later(function(){if((node=doc.getElementById(id))&&(fn(node)||1)||++retryCount>POLL_RETIRES){timer.cancel();}},POLL_INTERVAL,true);}});function _bindReady(){var doScroll=docElem.doScroll,eventType=doScroll?'onreadystatechange':'DOMContentLoaded',COMPLETE='complete',fire=function(){readyDefer.resolve(S)};if(doc.readyState===COMPLETE){return fire();}
if(doc.addEventListener){function domReady(){doc.removeEventListener(eventType,domReady,false);fire();}
doc.addEventListener(eventType,domReady,false);win.addEventListener('load',fire,false);}
else{function stateChange(){if(doc.readyState===COMPLETE){doc.detachEvent(eventType,stateChange);fire();}}
doc.attachEvent(eventType,stateChange);win.attachEvent('onload',fire);var notframe;try{notframe=(win['frameElement']===null);}catch(e){S.log('get frameElement error : ');S.log(e);notframe=false;}
if(doScroll&&notframe){function readyScroll(){try{doScroll('left');fire();}catch(ex){setTimeout(readyScroll,POLL_INTERVAL);}}
readyScroll();}}
return 0;}
if(location&&(location.search||EMPTY).indexOf('ks-debug')!==-1){S.Config.debug=true;}
_bindReady();if(navigator&&navigator.userAgent.match(/MSIE/)){try{doc.execCommand('BackgroundImageCache',false,true);}catch(e){}}})(KISSY,undefined);(function(S){S.config({packages:{gallery:{base:S.Config.baseUri.resolve('../').toString()}},modules:{core:{alias:['dom','event','ajax','anim','base','node','json']}}});})(KISSY);KISSY.config('modules',{'flash':{requires:['ua','dom','json']},'combobox':{requires:['component','node','input-selection','menu','ajax']},'anim':{requires:['dom','event','ua']},'toolbar':{requires:['component','node']},'dom':{requires:['ua']},'menubutton':{requires:['menu','node','button','component']},'waterfall':{requires:['node','base']},'dd':{requires:['ua','dom','event','node','base']},'switchable':{requires:['dom','anim','event']},'tree':{requires:['node','component','event']},'button':{requires:['component','event']},'component':{requires:['ua','node','event','dom','base']},'json':{requires:['ua']},'event':{requires:['ua','dom']},'ajax':{requires:['json','event','dom']},'resizable':{requires:['node','base','dd']},'stylesheet':{requires:['dom']},'input-selection':{requires:['dom']},'datalazyload':{requires:['dom','event','base']},'calendar':{requires:['node','ua','event']},'validation':{requires:['dom','event','node']},'imagezoom':{requires:['node','overlay']},'menu':{requires:['event','component','node','ua']},'suggest':{requires:['dom','event','ua']},'node':{requires:['event','dom','anim']},'editor':{requires:['htmlparser','component','core']},'split-button':{requires:['component','button','menubutton']},'mvc':{requires:['base','event','node','ajax','json']},'overlay':{requires:['ua','component','node']},'base':{requires:['event']},'separator':{requires:['component']},'tabs':{requires:['button','component','toolbar']}});KISSY.add('ua/base',function(S,undefined){var win=S.Env.host,navigator=win.navigator,ua=navigator.userAgent,EMPTY='',MOBILE='mobile',core=EMPTY,shell=EMPTY,m,IE_DETECT_RANGE=[6,9],v,end,VERSION_PLACEHOLDER='{{version}}',IE_DETECT_TPL='<!--[if IE '+VERSION_PLACEHOLDER+']><'+'s></s><![endif]-->',div=win.document.createElement('div'),s,UA={webkit:undefined,trident:undefined,gecko:undefined,presto:undefined,chrome:undefined,safari:undefined,firefox:undefined,ie:undefined,opera:undefined,mobile:undefined,core:undefined,shell:undefined},numberify=function(s){var c=0;return parseFloat(s.replace(/\./g,function(){return(c++===0)?'.':'';}));};div.innerHTML=IE_DETECT_TPL.replace(VERSION_PLACEHOLDER,'');s=div.getElementsByTagName('s');if(s.length>0){shell='ie';UA[core='trident']=0.1;if((m=ua.match(/Trident\/([\d.]*)/))&&m[1]){UA[core]=numberify(m[1]);}
for(v=IE_DETECT_RANGE[0],end=IE_DETECT_RANGE[1];v<=end;v++){div.innerHTML=IE_DETECT_TPL.replace(VERSION_PLACEHOLDER,v);if(s.length>0){UA[shell]=v;break;}}}else{if((m=ua.match(/AppleWebKit\/([\d.]*)/))&&m[1]){UA[core='webkit']=numberify(m[1]);if((m=ua.match(/Chrome\/([\d.]*)/))&&m[1]){UA[shell='chrome']=numberify(m[1]);}
else if((m=ua.match(/\/([\d.]*) Safari/))&&m[1]){UA[shell='safari']=numberify(m[1]);}
if(/ Mobile\//.test(ua)){UA[MOBILE]='apple';}
else if((m=ua.match(/NokiaN[^\/]*|Android \d\.\d|webOS\/\d\.\d/))){UA[MOBILE]=m[0].toLowerCase();}}
else{if((m=ua.match(/Presto\/([\d.]*)/))&&m[1]){UA[core='presto']=numberify(m[1]);if((m=ua.match(/Opera\/([\d.]*)/))&&m[1]){UA[shell='opera']=numberify(m[1]);if((m=ua.match(/Opera\/.* Version\/([\d.]*)/))&&m[1]){UA[shell]=numberify(m[1]);}
if((m=ua.match(/Opera Mini[^;]*/))&&m){UA[MOBILE]=m[0].toLowerCase();}
else if((m=ua.match(/Opera Mobi[^;]*/))&&m){UA[MOBILE]=m[0];}}}else{if((m=ua.match(/MSIE\s([^;]*)/))&&m[1]){UA[core='trident']=0.1;UA[shell='ie']=numberify(m[1]);if((m=ua.match(/Trident\/([\d.]*)/))&&m[1]){UA[core]=numberify(m[1]);}}else{if((m=ua.match(/Gecko/))){UA[core='gecko']=0.1;if((m=ua.match(/rv:([\d.]*)/))&&m[1]){UA[core]=numberify(m[1]);}
if((m=ua.match(/Firefox\/([\d.]*)/))&&m[1]){UA[shell='firefox']=numberify(m[1]);}}}}}}
UA.core=core;UA.shell=shell;UA._numberify=numberify;return UA;});KISSY.add('ua/css',function(S,UA){var o=['webkit','trident','gecko','presto','chrome','safari','firefox','ie','opera'],documentElement=S.Env.host.document.documentElement,className='',v;S.each(o,function(key){if(v=UA[key]){className+=' ks-'+key+(parseInt(v)+'');className+=' ks-'+key;}});documentElement.className=S.trim(documentElement.className+className);},{requires:['./base']});KISSY.add('ua/extra',function(S,UA,undefined){var win=S.Env.host,navigator=win.navigator,ua=navigator.userAgent,m,external,shell,o={se360:undefined,maxthon:undefined,tt:undefined,theworld:undefined,sougou:undefined},numberify=UA._numberify;if(m=ua.match(/360SE/)){o[shell='se360']=3;}
else if((m=ua.match(/Maxthon/))&&(external=win.external)){shell='maxthon';try{o[shell]=numberify(external['max_version']);}catch(ex){o[shell]=0.1;}}
else if(m=ua.match(/TencentTraveler\s([\d.]*)/)){o[shell='tt']=m[1]?numberify(m[1]):0.1;}
else if(m=ua.match(/TheWorld/)){o[shell='theworld']=3;}
else if(m=ua.match(/SE\s([\d.]*)/)){o[shell='sougou']=m[1]?numberify(m[1]):0.1;}
shell&&(o.shell=shell);S.mix(UA,o);return UA;},{requires:['ua/base']});KISSY.add('ua',function(S,UA){S.UA=UA;return UA;},{requires:['ua/extra','ua/css']});KISSY.add('dom/attr',function(S,DOM,UA,undefined){var doc=S.Env.host.document,NodeType=DOM.NodeType,docElement=doc.documentElement,IE_VERSION=UA.ie&&(doc.documentMode||UA.ie),TEXT=docElement.textContent===undefined?'innerText':'textContent',EMPTY='',HREF='href',nodeName=DOM.nodeName,R_BOOLEAN=/^(?:autofocus|autoplay|async|checked|controls|defer|disabled|hidden|loop|multiple|open|readonly|required|scoped|selected)$/i,R_FOCUSABLE=/^(?:button|input|object|select|textarea)$/i,R_CLICKABLE=/^a(?:rea)?$/i,R_INVALID_CHAR=/:|^on/,R_RETURN=/\r/g,attrFix={},attrFn={val:1,css:1,html:1,text:1,data:1,width:1,height:1,offset:1,scrollTop:1,scrollLeft:1},attrHooks={tabindex:{get:function(el){var attributeNode=el.getAttributeNode('tabindex');return attributeNode&&attributeNode.specified?parseInt(attributeNode.value,10):R_FOCUSABLE.test(el.nodeName)||R_CLICKABLE.test(el.nodeName)&&el.href?0:undefined;}},style:{get:function(el){return el.style.cssText;},set:function(el,val){el.style.cssText=val;}}},propFix={'hidefocus':'hideFocus',tabindex:'tabIndex',readonly:'readOnly','for':'htmlFor','class':'className',maxlength:'maxLength','cellspacing':'cellSpacing','cellpadding':'cellPadding',rowspan:'rowSpan',colspan:'colSpan',usemap:'useMap','frameborder':'frameBorder','contenteditable':'contentEditable'},boolHook={get:function(elem,name){return DOM.prop(elem,name)?name.toLowerCase():undefined;},set:function(elem,value,name){var propName;if(value===false){DOM.removeAttr(elem,name);}else{propName=propFix[name]||name;if(propName in elem){elem[propName]=true;}
elem.setAttribute(name,name.toLowerCase());}
return name;}},propHooks={},attrNodeHook={},valHooks={option:{get:function(elem){var val=elem.attributes.value;return!val||val.specified?elem.value:elem.text;}},select:{get:function(elem){var index=elem.selectedIndex,options=elem.options,one=elem.type==='select-one';if(index<0){return null;}else if(one){return DOM.val(options[index]);}
var ret=[],i=0,len=options.length;for(;i<len;++i){if(options[i].selected){ret.push(DOM.val(options[i]));}}
return ret;},set:function(elem,value){var values=S.makeArray(value),opts=elem.options;S.each(opts,function(opt){opt.selected=S.inArray(DOM.val(opt),values);});if(!values.length){elem.selectedIndex=-1;}
return values;}}};if(IE_VERSION&&IE_VERSION<8){attrNodeHook={get:function(elem,name){var ret;ret=elem.getAttributeNode(name);return ret&&(ret.specified||ret.nodeValue)?ret.nodeValue:undefined;},set:function(elem,value,name){var ret=elem.getAttributeNode(name);if(ret){ret.nodeValue=value;}else{try{var attr=elem.ownerDocument.createAttribute(name);attr.value=value;elem.setAttributeNode(attr);}
catch(e){return elem.setAttribute(name,value,0);}}}};attrFix=propFix;attrHooks.tabIndex=attrHooks.tabindex;S.each([HREF,'src','width','height','colSpan','rowSpan'],function(name){attrHooks[name]={get:function(elem){var ret=elem.getAttribute(name,2);return ret===null?undefined:ret;}};});valHooks.button=attrHooks.value=attrNodeHook;}
if(IE_VERSION&&IE_VERSION<9){var hrefFix=attrHooks[HREF]=attrHooks[HREF]||{};hrefFix.set=function(el,val,name){var childNodes=el.childNodes,b,len=childNodes.length,allText=len>0;for(len=len-1;len>=0;len--){if(childNodes[len].nodeType!=NodeType.TEXT_NODE){allText=0;}}
if(allText){b=el.ownerDocument.createElement('b');b.style.display='none';el.appendChild(b);}
el.setAttribute(name,EMPTY+val);if(b){el.removeChild(b);}};}
S.each(['radio','checkbox'],function(r){valHooks[r]={get:function(elem){return elem.getAttribute('value')===null?'on':elem.value;},set:function(elem,value){if(S.isArray(value)){return elem.checked=S.inArray(DOM.val(elem),value);}}};});function getProp(elem,name){name=propFix[name]||name;var hook=propHooks[name];if(hook&&hook.get){return hook.get(elem,name);}else{return elem[name];}}
S.mix(DOM,{prop:function(selector,name,value){var elems=DOM.query(selector);if(S.isPlainObject(name)){S.each(name,function(v,k){DOM.prop(elems,k,v);});return undefined;}
name=propFix[name]||name;var hook=propHooks[name];if(value!==undefined){for(var i=elems.length-1;i>=0;i--){var elem=elems[i];if(hook&&hook.set){hook.set(elem,value,name);}else{elem[name]=value;}}}else{if(elems.length){return getProp(elems[0],name);}}
return undefined;},hasProp:function(selector,name){var elems=DOM.query(selector);for(var i=0;i<elems.length;i++){var el=elems[i];if(getProp(el,name)!==undefined){return true;}}
return false;},removeProp:function(selector,name){name=propFix[name]||name;var elems=DOM.query(selector);for(var i=elems.length-1;i>=0;i--){var el=elems[i];try{el[name]=undefined;delete el[name];}catch(e){}}},attr:function(selector,name,val,pass){var els=DOM.query(selector);if(S.isPlainObject(name)){pass=val;for(var k in name){if(name.hasOwnProperty(k)){DOM.attr(els,k,name[k],pass);}}
return undefined;}
if(!(name=S.trim(name))){return undefined;}
if(pass&&attrFn[name]){return DOM[name](selector,val);}
name=name.toLowerCase();if(pass&&attrFn[name]){return DOM[name](selector,val);}
name=attrFix[name]||name;var attrNormalizer,el=els[0],ret;if(R_BOOLEAN.test(name)){attrNormalizer=boolHook;}
else if(R_INVALID_CHAR.test(name)){attrNormalizer=attrNodeHook;}else{attrNormalizer=attrHooks[name];}
if(val===undefined){if(el&&el.nodeType===NodeType.ELEMENT_NODE){if(nodeName(el)=='form'){attrNormalizer=attrNodeHook;}
if(attrNormalizer&&attrNormalizer.get){return attrNormalizer.get(el,name);}
ret=el.getAttribute(name);return ret===null?undefined:ret;}}else{for(var i=els.length-1;i>=0;i--){el=els[i];if(el&&el.nodeType===NodeType.ELEMENT_NODE){if(nodeName(el)=='form'){attrNormalizer=attrNodeHook;}
if(attrNormalizer&&attrNormalizer.set){attrNormalizer.set(el,val,name);}else{el.setAttribute(name,EMPTY+val);}}}}
return undefined;},removeAttr:function(selector,name){name=name.toLowerCase();name=attrFix[name]||name;var els=DOM.query(selector),el,i;for(i=els.length-1;i>=0;i--){el=els[i];if(el.nodeType==NodeType.ELEMENT_NODE){var propName;el.removeAttribute(name);if(R_BOOLEAN.test(name)&&(propName=propFix[name]||name)in el){el[propName]=false;}}}},hasAttr:!docElement.hasAttribute?function(selector,name){name=name.toLowerCase();var elems=DOM.query(selector);for(var i=0;i<elems.length;i++){var el=elems[i];var $attr=el.getAttributeNode(name);if($attr&&$attr.specified){return true;}}
return false;}:function(selector,name){var elems=DOM.query(selector);for(var i=0;i<elems.length;i++){var el=elems[i];if(el.hasAttribute(name)){return true;}}
return false;},val:function(selector,value){var hook,ret;if(value===undefined){var elem=DOM.get(selector);if(elem){hook=valHooks[nodeName(elem)]||valHooks[elem.type];if(hook&&'get'in hook&&(ret=hook.get(elem,'value'))!==undefined){return ret;}
ret=elem.value;return typeof ret==='string'?ret.replace(R_RETURN,''):ret==null?'':ret;}
return undefined;}
var els=DOM.query(selector),i;for(i=els.length-1;i>=0;i--){elem=els[i];if(elem.nodeType!==1){return undefined;}
var val=value;if(val==null){val='';}else if(typeof val==='number'){val+='';}else if(S.isArray(val)){val=S.map(val,function(value){return value==null?'':value+'';});}
hook=valHooks[nodeName(elem)]||valHooks[elem.type];if(!hook||!('set'in hook)||hook.set(elem,val,'value')===undefined){elem.value=val;}}
return undefined;},_propHooks:propHooks,text:function(selector,val){if(val===undefined){var el=DOM.get(selector);if(el.nodeType==NodeType.ELEMENT_NODE){return el[TEXT]||EMPTY;}
else if(el.nodeType==NodeType.TEXT_NODE){return el.nodeValue;}
return undefined;}
else{var els=DOM.query(selector),i;for(i=els.length-1;i>=0;i--){el=els[i];if(el.nodeType==NodeType.ELEMENT_NODE){el[TEXT]=val;}
else if(el.nodeType==NodeType.TEXT_NODE){el.nodeValue=val;}}}
return undefined;}});return DOM;},{requires:['./base','ua']});KISSY.add('dom/base',function(S,UA,undefined){var WINDOW=S.Env.host;var NodeType={ELEMENT_NODE:1,'ATTRIBUTE_NODE':2,TEXT_NODE:3,'CDATA_SECTION_NODE':4,'ENTITY_REFERENCE_NODE':5,'ENTITY_NODE':6,'PROCESSING_INSTRUCTION_NODE':7,COMMENT_NODE:8,DOCUMENT_NODE:9,'DOCUMENT_TYPE_NODE':10,DOCUMENT_FRAGMENT_NODE:11,'NOTATION_NODE':12};var DOM={isCustomDomain:function(win){win=win||WINDOW;var domain=win.document.domain,hostname=win.location.hostname;return domain!=hostname&&domain!=('['+hostname+']');},getEmptyIframeSrc:function(win){win=win||WINDOW;if(UA['ie']&&DOM.isCustomDomain(win)){return'javascript:void(function(){'+encodeURIComponent('document.open();'+"document.domain='"+
win.document.domain
+"';"+'document.close();')+'}())';}
return undefined;},NodeType:NodeType,_getWin:function(elem){if(elem==null){return WINDOW;}
return('scrollTo'in elem&&elem['document'])?elem:elem.nodeType==NodeType.DOCUMENT_NODE?elem.defaultView||elem.parentWindow:false;},_isNodeList:function(o){return o&&!o.nodeType&&o.item&&!o.setTimeout;},nodeName:function(selector){var el=DOM.get(selector),nodeName=el.nodeName.toLowerCase();if(UA['ie']){var scopeName=el['scopeName'];if(scopeName&&scopeName!='HTML'){nodeName=scopeName.toLowerCase()+':'+nodeName;}}
return nodeName;}};S.mix(DOM,NodeType);return DOM;},{requires:['ua']});KISSY.add('dom/class',function(S,DOM,undefined){var SPACE=' ',NodeType=DOM.NodeType,REG_SPLIT=/[\.\s]\s*\.?/,REG_CLASS=/[\n\t]/g;function norm(elemClass){return(SPACE+elemClass+SPACE).replace(REG_CLASS,SPACE);}
S.mix(DOM,{hasClass:function(selector,className){return batch(selector,className,function(elem,classNames,cl){var elemClass=elem.className;if(elemClass){var className=norm(elemClass),j=0,ret=true;for(;j<cl;j++){if(className.indexOf(SPACE+classNames[j]+SPACE)<0){ret=false;break;}}
if(ret){return true;}}},true);},addClass:function(selector,className){batch(selector,className,function(elem,classNames,cl){var elemClass=elem.className;if(!elemClass){elem.className=className;}else{var normClassName=norm(elemClass),setClass=elemClass,j=0;for(;j<cl;j++){if(normClassName.indexOf(SPACE+classNames[j]+SPACE)<0){setClass+=SPACE+classNames[j];}}
elem.className=S.trim(setClass);}},undefined);},removeClass:function(selector,className){batch(selector,className,function(elem,classNames,cl){var elemClass=elem.className;if(elemClass){if(!cl){elem.className='';}else{var className=norm(elemClass),j=0,needle;for(;j<cl;j++){needle=SPACE+classNames[j]+SPACE;while(className.indexOf(needle)>=0){className=className.replace(needle,SPACE);}}
elem.className=S.trim(className);}}},undefined);},replaceClass:function(selector,oldClassName,newClassName){DOM.removeClass(selector,oldClassName);DOM.addClass(selector,newClassName);},toggleClass:function(selector,className,state){var isBool=S.isBoolean(state),has;batch(selector,className,function(elem,classNames,cl){var j=0,className;for(;j<cl;j++){className=classNames[j];has=isBool?!state:DOM.hasClass(elem,className);DOM[has?'removeClass':'addClass'](elem,className);}},undefined);}});function batch(selector,value,fn,resultIsBool){if(!(value=S.trim(value))){return resultIsBool?false:undefined;}
var elems=DOM.query(selector),len=elems.length,tmp=value.split(REG_SPLIT),elem,ret;var classNames=[];for(var i=0;i<tmp.length;i++){var t=S.trim(tmp[i]);if(t){classNames.push(t);}}
for(i=0;i<len;i++){elem=elems[i];if(elem.nodeType==NodeType.ELEMENT_NODE){ret=fn(elem,classNames,classNames.length);if(ret!==undefined){return ret;}}}
if(resultIsBool){return false;}
return undefined;}
return DOM;},{requires:['dom/base']});KISSY.add('dom/create',function(S,DOM,UA,undefined){var doc=S.Env.host.document,NodeType=DOM.NodeType,ie=UA['ie'],isString=S.isString,DIV='div',PARENT_NODE='parentNode',DEFAULT_DIV=doc.createElement(DIV),R_XHTML_TAG=/<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/ig,RE_TAG=/<([\w:]+)/,R_TBODY=/<tbody/i,R_LEADING_WHITESPACE=/^\s+/,lostLeadingWhitespace=ie&&ie<9,R_HTML=/<|&#?\w+;/,supportOuterHTML='outerHTML'in doc.documentElement,RE_SIMPLE_TAG=/^<(\w+)\s*\/?>(?:<\/\1>)?$/;function getElementsByTagName(el,tag){return el.getElementsByTagName(tag);}
function cleanData(els){var Event=S.require('event');if(Event){Event.detach(els);}
DOM.removeData(els);}
S.mix(DOM,{create:function(html,props,ownerDoc,_trim){var ret=null;if(!html){return ret;}
if(html.nodeType){return DOM.clone(html);}
if(!isString(html)){return ret;}
if(_trim===undefined){_trim=true;}
if(_trim){html=S.trim(html);}
var creators=DOM._creators,holder,whitespaceMatch,context=ownerDoc||doc,m,tag=DIV,k,nodes;if(!R_HTML.test(html)){ret=context.createTextNode(html);}
else if((m=RE_SIMPLE_TAG.exec(html))){ret=context.createElement(m[1]);}
else{html=html.replace(R_XHTML_TAG,'<$1><'+'/$2>');if((m=RE_TAG.exec(html))&&(k=m[1])){tag=k.toLowerCase();}
holder=(creators[tag]||creators[DIV])(html,context);if(lostLeadingWhitespace&&(whitespaceMatch=html.match(R_LEADING_WHITESPACE))){holder.insertBefore(context.createTextNode(whitespaceMatch[0]),holder.firstChild);}
nodes=holder.childNodes;if(nodes.length===1){ret=nodes[0][PARENT_NODE].removeChild(nodes[0]);}
else if(nodes.length){ret=nodeListToFragment(nodes);}else{S.error(html+' : create node error');}}
return attachProps(ret,props);},_creators:{div:function(html,ownerDoc){var frag=ownerDoc&&ownerDoc!=doc?ownerDoc.createElement(DIV):DEFAULT_DIV;frag['innerHTML']='m<div>'+html+'<'+'/div>';return frag.lastChild;}},html:function(selector,htmlString,loadScripts,callback){var els=DOM.query(selector),el=els[0];if(!el){return}
if(htmlString===undefined){if(el.nodeType==NodeType.ELEMENT_NODE){return el.innerHTML;}else{return null;}}
else{var success=false,i,elem;htmlString+='';if(!htmlString.match(/<(?:script|style|link)/i)&&(!lostLeadingWhitespace||!htmlString.match(R_LEADING_WHITESPACE))&&!creatorsMap[(htmlString.match(RE_TAG)||['',''])[1].toLowerCase()]){try{for(i=els.length-1;i>=0;i--){elem=els[i];if(elem.nodeType==NodeType.ELEMENT_NODE){cleanData(getElementsByTagName(elem,'*'));elem.innerHTML=htmlString;}}
success=true;}catch(e){}}
if(!success){var valNode=DOM.create(htmlString,0,el.ownerDocument,0);DOM.empty(els);DOM.append(valNode,els,loadScripts);}
callback&&callback();}},outerHTML:function(selector,htmlString,loadScripts){var els=DOM.query(selector),holder,i,valNode,length=els.length,el=els[0];if(!el){return}
if(htmlString===undefined){if(supportOuterHTML){return el.outerHTML}else{holder=el.ownerDocument.createElement('div');holder.appendChild(DOM.clone(el,true));return holder.innerHTML;}}else{htmlString+='';if(!htmlString.match(/<(?:script|style|link)/i)&&supportOuterHTML){for(i=length-1;i>=0;i--){el=els[i];if(el.nodeType==NodeType.ELEMENT_NODE){cleanData(el);cleanData(getElementsByTagName(el,'*'));el.outerHTML=htmlString;}}}else{valNode=DOM.create(htmlString,0,el.ownerDocument,0);DOM.insertBefore(valNode,els,loadScripts);DOM.remove(els);}}},remove:function(selector,keepData){var el,els=DOM.query(selector),i;for(i=els.length-1;i>=0;i--){el=els[i];if(!keepData&&el.nodeType==NodeType.ELEMENT_NODE){var elChildren=getElementsByTagName(el,'*');cleanData(elChildren);cleanData(el);}
if(el.parentNode){el.parentNode.removeChild(el);}}},clone:function(selector,deep,withDataAndEvent,deepWithDataAndEvent){if(typeof deep==='object'){deepWithDataAndEvent=deep['deepWithDataAndEvent'];withDataAndEvent=deep['withDataAndEvent'];deep=deep['deep'];}
var elem=DOM.get(selector);if(!elem){return null;}
var clone=elem.cloneNode(deep);if(elem.nodeType==NodeType.ELEMENT_NODE||elem.nodeType==NodeType.DOCUMENT_FRAGMENT_NODE){if(elem.nodeType==NodeType.ELEMENT_NODE){fixAttributes(elem,clone);}
if(deep){processAll(fixAttributes,elem,clone);}}
if(withDataAndEvent){cloneWithDataAndEvent(elem,clone);if(deep&&deepWithDataAndEvent){processAll(cloneWithDataAndEvent,elem,clone);}}
return clone;},empty:function(selector){var els=DOM.query(selector),el,i;for(i=els.length-1;i>=0;i--){el=els[i];DOM.remove(el.childNodes);}},nodeListToFragment:nodeListToFragment});function processAll(fn,elem,clone){if(elem.nodeType==NodeType.DOCUMENT_FRAGMENT_NODE){var eCs=elem.childNodes,cloneCs=clone.childNodes,fIndex=0;while(eCs[fIndex]){if(cloneCs[fIndex]){processAll(fn,eCs[fIndex],cloneCs[fIndex]);}
fIndex++;}}else if(elem.nodeType==NodeType.ELEMENT_NODE){var elemChildren=getElementsByTagName(elem,'*'),cloneChildren=getElementsByTagName(clone,'*'),cIndex=0;while(elemChildren[cIndex]){if(cloneChildren[cIndex]){fn(elemChildren[cIndex],cloneChildren[cIndex]);}
cIndex++;}}}
function cloneWithDataAndEvent(src,dest){var Event=S.require('event');if(dest.nodeType==NodeType.ELEMENT_NODE&&!DOM.hasData(src)){return;}
var srcData=DOM.data(src);for(var d in srcData){DOM.data(dest,d,srcData[d]);}
if(Event){Event._removeData(dest);Event._clone(src,dest);}}
function fixAttributes(src,dest){if(dest.clearAttributes){dest.clearAttributes();}
if(dest.mergeAttributes){dest.mergeAttributes(src);}
var nodeName=dest.nodeName.toLowerCase(),srcChilds=src.childNodes;if(nodeName==='object'&&!dest.childNodes.length){for(var i=0;i<srcChilds.length;i++){dest.appendChild(srcChilds[i].cloneNode(true));}}else if(nodeName==='input'&&(src.type==='checkbox'||src.type==='radio')){if(src.checked){dest['defaultChecked']=dest.checked=src.checked;}
if(dest.value!==src.value){dest.value=src.value;}}else if(nodeName==='option'){dest.selected=src.defaultSelected;}else if(nodeName==='input'||nodeName==='textarea'){dest.defaultValue=src.defaultValue;}
dest.removeAttribute(DOM.__EXPANDO);}
function attachProps(elem,props){if(S.isPlainObject(props)){if(elem.nodeType==NodeType.ELEMENT_NODE){DOM.attr(elem,props,true);}
else if(elem.nodeType==NodeType.DOCUMENT_FRAGMENT_NODE){DOM.attr(elem.childNodes,props,true);}}
return elem;}
function nodeListToFragment(nodes){var ret=null,i,ownerDoc,len;if(nodes&&(nodes.push||nodes.item)&&nodes[0]){ownerDoc=nodes[0].ownerDocument;ret=ownerDoc.createDocumentFragment();nodes=S.makeArray(nodes);for(i=0,len=nodes.length;i<len;i++){ret.appendChild(nodes[i]);}}else{S.log('Unable to convert '+nodes+' to fragment.');}
return ret;}
var creators=DOM._creators,create=DOM.create,creatorsMap={option:'select',optgroup:'select',area:'map',thead:'table',td:'tr',th:'tr',tr:'tbody',tbody:'table',tfoot:'table',caption:'table',colgroup:'table',col:'colgroup',legend:'fieldset'};for(var p in creatorsMap){(function(tag){creators[p]=function(html,ownerDoc){return create('<'+tag+'>'+html+'<'+'/'+tag+'>',null,ownerDoc);};})(creatorsMap[p]);}
if(ie<8){creators.table=function(html,ownerDoc){var frag=creators[DIV](html,ownerDoc),hasTBody=R_TBODY.test(html);if(hasTBody){return frag;}
var table=frag.firstChild,tableChildren=S.makeArray(table.childNodes);S.each(tableChildren,function(c){if(DOM.nodeName(c)=='tbody'&&!c.childNodes.length){table.removeChild(c);}});return frag;};}
return DOM;},{requires:['./base','ua']});KISSY.add('dom/data',function(S,DOM,undefined){var win=S.Env.host,EXPANDO='__ks_data_'+S.now(),dataCache={},winDataCache={};var noData={};noData['applet']=1;noData['object']=1;noData['embed']=1;var commonOps={hasData:function(cache,name){if(cache){if(name!==undefined){if(name in cache){return true;}}else if(!S.isEmptyObject(cache)){return true;}}
return false;}};var objectOps={hasData:function(ob,name){if(ob==win){return objectOps.hasData(winDataCache,name);}
var thisCache=ob[EXPANDO];return commonOps.hasData(thisCache,name);},data:function(ob,name,value){if(ob==win){return objectOps.data(winDataCache,name,value);}
var cache=ob[EXPANDO];if(value!==undefined){cache=ob[EXPANDO]=ob[EXPANDO]||{};cache[name]=value;}else{if(name!==undefined){return cache&&cache[name];}else{cache=ob[EXPANDO]=ob[EXPANDO]||{};return cache;}}},removeData:function(ob,name){if(ob==win){return objectOps.removeData(winDataCache,name);}
var cache=ob[EXPANDO];if(name!==undefined){delete cache[name];if(S.isEmptyObject(cache)){objectOps.removeData(ob);}}else{try{delete ob[EXPANDO];}catch(e){ob[EXPANDO]=undefined;}}}};var domOps={hasData:function(elem,name){var key=elem[EXPANDO];if(!key){return false;}
var thisCache=dataCache[key];return commonOps.hasData(thisCache,name);},data:function(elem,name,value){if(noData[elem.nodeName.toLowerCase()]){return undefined;}
var key=elem[EXPANDO],cache;if(!key){if(name!==undefined&&value===undefined){return undefined;}
key=elem[EXPANDO]=S.guid();}
cache=dataCache[key];if(value!==undefined){cache=dataCache[key]=dataCache[key]||{};cache[name]=value;}else{if(name!==undefined){return cache&&cache[name];}else{cache=dataCache[key]=dataCache[key]||{};return cache;}}},removeData:function(elem,name){var key=elem[EXPANDO],cache;if(!key){return;}
cache=dataCache[key];if(name!==undefined){delete cache[name];if(S.isEmptyObject(cache)){domOps.removeData(elem);}}else{delete dataCache[key];try{delete elem[EXPANDO];}catch(e){elem[EXPANDO]=undefined;}
if(elem.removeAttribute){elem.removeAttribute(EXPANDO);}}}};S.mix(DOM,{__EXPANDO:EXPANDO,hasData:function(selector,name){var ret=false,elems=DOM.query(selector);for(var i=0;i<elems.length;i++){var elem=elems[i];if(elem.nodeType){ret=domOps.hasData(elem,name);}else{ret=objectOps.hasData(elem,name);}
if(ret){return ret;}}
return ret;},data:function(selector,name,data){var elems=DOM.query(selector),elem=elems[0];if(S.isPlainObject(name)){for(var k in name){if(name.hasOwnProperty(k)){DOM.data(elems,k,name[k]);}}
return undefined;}
if(data===undefined){if(elem){if(elem.nodeType){return domOps.data(elem,name);}else{return objectOps.data(elem,name);}}}
else{for(var i=elems.length-1;i>=0;i--){elem=elems[i];if(elem.nodeType){domOps.data(elem,name,data);}else{objectOps.data(elem,name,data);}}}
return undefined;},removeData:function(selector,name){var els=DOM.query(selector),elem,i;for(i=els.length-1;i>=0;i--){elem=els[i];if(elem.nodeType){domOps.removeData(elem,name);}else{objectOps.removeData(elem,name);}}}});return DOM;},{requires:['./base']});KISSY.add('dom',function(S,DOM){S.mix(S,{DOM:DOM,get:DOM.get,query:DOM.query});return DOM;},{requires:['dom/attr','dom/class','dom/create','dom/data','dom/insertion','dom/offset','dom/style','dom/selector','dom/style-ie','dom/traversal']});KISSY.add('dom/insertion',function(S,UA,DOM){var PARENT_NODE='parentNode',NodeType=DOM.NodeType,R_FORM_EL=/^(?:button|input|object|select|textarea)$/i,getNodeName=DOM.nodeName,makeArray=S.makeArray,splice=[].splice,NEXT_SIBLING='nextSibling';function fixChecked(ret){for(var i=0;i<ret.length;i++){var el=ret[i];if(el.nodeType==NodeType.DOCUMENT_FRAGMENT_NODE){fixChecked(el.childNodes);}else if(getNodeName(el)=='input'){fixCheckedInternal(el);}else if(el.nodeType==NodeType.ELEMENT_NODE){var cs=el.getElementsByTagName('input');for(var j=0;j<cs.length;j++){fixChecked(cs[j]);}}}}
function fixCheckedInternal(el){if(el.type==='checkbox'||el.type==='radio'){el.defaultChecked=el.checked;}}
var R_SCRIPT_TYPE=/\/(java|ecma)script/i;function isJs(el){return!el.type||R_SCRIPT_TYPE.test(el.type);}
function filterScripts(nodes,scripts){var ret=[],i,el,nodeName;for(i=0;nodes[i];i++){el=nodes[i];nodeName=getNodeName(el);if(el.nodeType==NodeType.DOCUMENT_FRAGMENT_NODE){ret.push.apply(ret,filterScripts(makeArray(el.childNodes),scripts));}else if(nodeName==='script'&&isJs(el)){if(el.parentNode){el.parentNode.removeChild(el)}
if(scripts){scripts.push(el);}}else{if(el.nodeType==NodeType.ELEMENT_NODE&&!R_FORM_EL.test(nodeName)){var tmp=[],s,j,ss=el.getElementsByTagName('script');for(j=0;j<ss.length;j++){s=ss[j];if(isJs(s)){tmp.push(s);}}
splice.apply(nodes,[i+1,0].concat(tmp));}
ret.push(el);}}
return ret;}
function evalScript(el){if(el.src){S.getScript(el.src);}else{var code=S.trim(el.text||el.textContent||el.innerHTML||'');if(code){S.globalEval(code);}}}
function insertion(newNodes,refNodes,fn,scripts){newNodes=DOM.query(newNodes);if(scripts){scripts=[];}
newNodes=filterScripts(newNodes,scripts);if(UA['ie']<8){fixChecked(newNodes);}
refNodes=DOM.query(refNodes);var newNodesLength=newNodes.length,refNodesLength=refNodes.length;if((!newNodesLength&&(!scripts||!scripts.length))||!refNodesLength){return;}
var newNode=DOM.nodeListToFragment(newNodes),clonedNode;if(refNodesLength>1){clonedNode=DOM.clone(newNode,true);refNodes=S.makeArray(refNodes)}
for(var i=0;i<refNodesLength;i++){var refNode=refNodes[i];if(newNode){var node=i>0?DOM.clone(clonedNode,true):newNode;fn(node,refNode);}
if(scripts&&scripts.length){S.each(scripts,evalScript);}}}
S.mix(DOM,{insertBefore:function(newNodes,refNodes,loadScripts){insertion(newNodes,refNodes,function(newNode,refNode){if(refNode[PARENT_NODE]){refNode[PARENT_NODE].insertBefore(newNode,refNode);}},loadScripts);},insertAfter:function(newNodes,refNodes,loadScripts){insertion(newNodes,refNodes,function(newNode,refNode){if(refNode[PARENT_NODE]){refNode[PARENT_NODE].insertBefore(newNode,refNode[NEXT_SIBLING]);}},loadScripts);},appendTo:function(newNodes,parents,loadScripts){insertion(newNodes,parents,function(newNode,parent){parent.appendChild(newNode);},loadScripts);},prependTo:function(newNodes,parents,loadScripts){insertion(newNodes,parents,function(newNode,parent){parent.insertBefore(newNode,parent.firstChild);},loadScripts);},wrapAll:function(wrappedNodes,wrapperNode){wrapperNode=DOM.clone(DOM.get(wrapperNode),true);wrappedNodes=DOM.query(wrappedNodes);if(wrappedNodes[0].parentNode){DOM.insertBefore(wrapperNode,wrappedNodes[0]);}
var c;while((c=wrapperNode.firstChild)&&c.nodeType==1){wrapperNode=c;}
DOM.appendTo(wrappedNodes,wrapperNode);},wrap:function(wrappedNodes,wrapperNode){wrappedNodes=DOM.query(wrappedNodes);wrapperNode=DOM.get(wrapperNode);S.each(wrappedNodes,function(w){DOM.wrapAll(w,wrapperNode);});},wrapInner:function(wrappedNodes,wrapperNode){wrappedNodes=DOM.query(wrappedNodes);wrapperNode=DOM.get(wrapperNode);S.each(wrappedNodes,function(w){var contents=w.childNodes;if(contents.length){DOM.wrapAll(contents,wrapperNode);}else{w.appendChild(wrapperNode);}});},unwrap:function(wrappedNodes){wrappedNodes=DOM.query(wrappedNodes);S.each(wrappedNodes,function(w){var p=w.parentNode;DOM.replaceWith(p,p.childNodes);});},replaceWith:function(selector,newNodes){var nodes=DOM.query(selector);newNodes=DOM.query(newNodes);DOM.remove(newNodes,true);DOM.insertBefore(newNodes,nodes);DOM.remove(nodes);}});S.each({'prepend':'prependTo','append':'appendTo','before':'insertBefore','after':'insertAfter'},function(value,key){DOM[key]=DOM[value];});return DOM;},{requires:['ua','./create']});KISSY.add('dom/offset',function(S,DOM,UA,undefined){var win=S.Env.host,doc=win.document,NodeType=DOM.NodeType,docElem=doc.documentElement,getWin=DOM._getWin,CSS1Compat='CSS1Compat',compatMode='compatMode',MAX=Math.max,myParseInt=parseInt,POSITION='position',RELATIVE='relative',DOCUMENT='document',BODY='body',DOC_ELEMENT='documentElement',OWNER_DOCUMENT='ownerDocument',VIEWPORT='viewport',SCROLL='scroll',CLIENT='client',LEFT='left',TOP='top',isNumber=S.isNumber,SCROLL_LEFT=SCROLL+'Left',SCROLL_TOP=SCROLL+'Top';S.mix(DOM,{offset:function(selector,coordinates,relativeWin){if(coordinates===undefined){var elem=DOM.get(selector),ret;if(elem){ret=getOffset(elem,relativeWin);}
return ret;}
var els=DOM.query(selector),i;for(i=els.length-1;i>=0;i--){elem=els[i];setOffset(elem,coordinates);}
return undefined;},scrollIntoView:function(selector,container,top,hscroll,auto){var elem;if(!(elem=DOM.get(selector))){return;}
if(container){container=DOM.get(container);}
if(!container){container=elem.ownerDocument;}
if(auto!==true){hscroll=hscroll===undefined?true:!!hscroll;top=top===undefined?true:!!top;}
if(container.nodeType==NodeType.DOCUMENT_NODE){container=getWin(container);}
var isWin=!!getWin(container),elemOffset=DOM.offset(elem),eh=DOM.outerHeight(elem),ew=DOM.outerWidth(elem),containerOffset,ch,cw,containerScroll,diffTop,diffBottom,win,winScroll,ww,wh;if(isWin){win=container;wh=DOM.height(win);ww=DOM.width(win);winScroll={left:DOM.scrollLeft(win),top:DOM.scrollTop(win)};diffTop={left:elemOffset[LEFT]-winScroll[LEFT],top:elemOffset[TOP]-winScroll[TOP]};diffBottom={left:elemOffset[LEFT]+ew-(winScroll[LEFT]+ww),top:elemOffset[TOP]+eh-(winScroll[TOP]+wh)};containerScroll=winScroll;}
else{containerOffset=DOM.offset(container);ch=container.clientHeight;cw=container.clientWidth;containerScroll={left:DOM.scrollLeft(container),top:DOM.scrollTop(container)};diffTop={left:elemOffset[LEFT]-containerOffset[LEFT]-
(myParseInt(DOM.css(container,'borderLeftWidth'))||0),top:elemOffset[TOP]-containerOffset[TOP]-
(myParseInt(DOM.css(container,'borderTopWidth'))||0)};diffBottom={left:elemOffset[LEFT]+ew-
(containerOffset[LEFT]+cw+
(myParseInt(DOM.css(container,'borderRightWidth'))||0)),top:elemOffset[TOP]+eh-
(containerOffset[TOP]+ch+
(myParseInt(DOM.css(container,'borderBottomWidth'))||0))};}
if(diffTop.top<0||diffBottom.top>0){if(top===true){DOM.scrollTop(container,containerScroll.top+diffTop.top);}else if(top===false){DOM.scrollTop(container,containerScroll.top+diffBottom.top);}else{if(diffTop.top<0){DOM.scrollTop(container,containerScroll.top+diffTop.top);}else{DOM.scrollTop(container,containerScroll.top+diffBottom.top);}}}
if(hscroll){if(diffTop.left<0||diffBottom.left>0){if(top===true){DOM.scrollLeft(container,containerScroll.left+diffTop.left);}else if(top===false){DOM.scrollLeft(container,containerScroll.left+diffBottom.left);}else{if(diffTop.left<0){DOM.scrollLeft(container,containerScroll.left+diffTop.left);}else{DOM.scrollLeft(container,containerScroll.left+diffBottom.left);}}}}},docWidth:0,docHeight:0,viewportHeight:0,viewportWidth:0,scrollTop:0,scrollLeft:0});S.each(['Left','Top'],function(name,i){var method=SCROLL+name;DOM[method]=function(elem,v){if(isNumber(elem)){return arguments.callee(win,elem);}
elem=DOM.get(elem);var ret,w=getWin(elem),d;if(w){if(v!==undefined){v=parseFloat(v);var left=name=='Left'?v:DOM.scrollLeft(w),top=name=='Top'?v:DOM.scrollTop(w);w['scrollTo'](left,top);}else{ret=w['page'+(i?'Y':'X')+'Offset'];if(!isNumber(ret)){d=w[DOCUMENT];ret=d[DOC_ELEMENT][method];if(!isNumber(ret)){ret=d[BODY][method];}}}}else if(elem.nodeType==NodeType.ELEMENT_NODE){if(v!==undefined){elem[method]=parseFloat(v)}else{ret=elem[method];}}
return ret;}});S.each(['Width','Height'],function(name){DOM['doc'+name]=function(refWin){refWin=DOM.get(refWin);var w=getWin(refWin),d=w[DOCUMENT];return MAX(d[DOC_ELEMENT][SCROLL+name],d[BODY][SCROLL+name],DOM[VIEWPORT+name](d));};DOM[VIEWPORT+name]=function(refWin){refWin=DOM.get(refWin);var prop=CLIENT+name,win=getWin(refWin),doc=win[DOCUMENT],body=doc[BODY],documentElement=doc[DOC_ELEMENT],documentElementProp=documentElement[prop];return doc[compatMode]===CSS1Compat&&documentElementProp||body&&body[prop]||documentElementProp;}});function getClientPosition(elem){var box,x,y,doc=elem.ownerDocument,body=doc.body;box=elem.getBoundingClientRect();x=box[LEFT];y=box[TOP];x-=docElem.clientLeft||body.clientLeft||0;y-=docElem.clientTop||body.clientTop||0;return{left:x,top:y};}
function getPageOffset(el){var pos=getClientPosition(el);var w=getWin(el[OWNER_DOCUMENT]);pos.left+=DOM[SCROLL_LEFT](w);pos.top+=DOM[SCROLL_TOP](w);return pos;}
function getOffset(el,relativeWin){var position={left:0,top:0};var currentWin=getWin(el[OWNER_DOCUMENT]);var currentEl=el;relativeWin=relativeWin||currentWin;do{var offset=currentWin==relativeWin?getPageOffset(currentEl):getClientPosition(currentEl);position.left+=offset.left;position.top+=offset.top;}while(currentWin&&currentWin!=relativeWin&&(currentEl=currentWin['frameElement'])&&(currentWin=currentWin.parent));return position;}
function setOffset(elem,offset){if(DOM.css(elem,POSITION)==='static'){elem.style[POSITION]=RELATIVE;}
var old=getOffset(elem),ret={},current,key;for(key in offset){if(offset.hasOwnProperty(key)){current=myParseInt(DOM.css(elem,key),10)||0;ret[key]=current+offset[key]-old[key];}}
DOM.css(elem,ret);}
return DOM;},{requires:['./base','ua']});KISSY.add('dom/selector',function(S,DOM,undefined){var doc=S.Env.host.document,NodeType=DOM.NodeType,filter=S.filter,require=function(selector){return S.require(selector);},isArray=S.isArray,isString=S.isString,makeArray=S.makeArray,isNodeList=DOM._isNodeList,getNodeName=DOM.nodeName,push=Array.prototype.push,SPACE=' ',COMMA=',',trim=S.trim,ANY='*',REG_ID=/^#[\w-]+$/,REG_QUERY=/^(?:#([\w-]+))?\s*([\w-]+|\*)?\.?([\w-]+)?$/;function query_each(f){var self=this,el,i;for(i=0;i<self.length;i++){el=self[i];if(f(el,i)===false){break;}}}
function query(selector,context){var ret,i,simpleContext,isSelectorString=isString(selector),contexts=(context===undefined&&(simpleContext=1))?[doc]:tuneContext(context);if(!selector){ret=[];}
else if(isSelectorString){selector=trim(selector);if(simpleContext&&selector=='body'){ret=[doc.body]}else if(contexts.length==1&&selector){ret=quickFindBySelectorStr(selector,contexts[0]);}}
else if(simpleContext){if(selector['nodeType']||selector['setTimeout']){ret=[selector];}
else if(selector['getDOMNodes']){return selector;}
else if(isArray(selector)){ret=selector;}
else if(isNodeList(selector)){ret=S.makeArray(selector);}else{ret=[selector];}}
if(!ret){ret=[];if(selector){for(i=0;i<contexts.length;i++){push.apply(ret,queryByContexts(selector,contexts[i]));}
if(ret.length>1&&(contexts.length>1||(isSelectorString&&selector.indexOf(COMMA)>-1))){unique(ret);}}}
ret.each=query_each;return ret;}
function queryByContexts(selector,context){var ret=[],isSelectorString=isString(selector);if(isSelectorString&&selector.match(REG_QUERY)||!isSelectorString){ret=queryBySimple(selector,context);}
else if(isSelectorString&&selector.indexOf(COMMA)>-1){ret=queryBySelectors(selector,context);}
else{ret=queryBySizzle(selector,context);}
return ret;}
function queryBySizzle(selector,context){var ret=[],sizzle=require('sizzle');if(sizzle){sizzle(selector,context,ret);}else{error(selector);}
return ret;}
function queryBySelectors(selector,context){var ret=[],i,selectors=selector.split(/\s*,\s*/);for(i=0;i<selectors.length;i++){push.apply(ret,queryByContexts(selectors[i],context));}
return ret;}
function quickFindBySelectorStr(selector,context){var ret,t,match,id,tag,cls;if(REG_ID.test(selector)){t=getElementById(selector.slice(1),context);if(t){ret=[t];}else{ret=[];}}
else{match=REG_QUERY.exec(selector);if(match){id=match[1];tag=match[2];cls=match[3];context=(id?getElementById(id,context):context);if(context){if(cls){if(!id||selector.indexOf(SPACE)!=-1){ret=[].concat(getElementsByClassName(cls,tag,context));}
else{t=getElementById(id,context);if(t&&hasClass(t,cls)){ret=[t];}}}
else if(tag){ret=getElementsByTagName(tag,context);}}
ret=ret||[];}}
return ret;}
function queryBySimple(selector,context){var ret,isSelectorString=isString(selector);if(isSelectorString){ret=quickFindBySelectorStr(selector,context)||[];}
else if(isArray(selector)||isNodeList(selector)){ret=filter(selector,function(s){return testByContext(s,context);});}
else if(testByContext(selector,context)){ret=[selector];}
return ret;}
function testByContext(element,context){if(!element){return false;}
if(context==doc){return true;}
return DOM.contains(context,element);}
var unique=S.noop;(function(){var sortOrder,hasDuplicate,baseHasDuplicate=true;[0,0].sort(function(){baseHasDuplicate=false;return 0;});unique=function(elements){if(sortOrder){hasDuplicate=baseHasDuplicate;elements.sort(sortOrder);if(hasDuplicate){var i=1,len=elements.length;while(i<len){if(elements[i]===elements[i-1]){elements.splice(i,1);}else{i++;}}}}
return elements;};if(doc.documentElement.compareDocumentPosition){sortOrder=function(a,b){if(a==b){hasDuplicate=true;return 0;}
if(!a.compareDocumentPosition||!b.compareDocumentPosition){return a.compareDocumentPosition?-1:1;}
return a.compareDocumentPosition(b)&4?-1:1;};}else{sortOrder=function(a,b){if(a==b){hasDuplicate=true;return 0;}else if(a.sourceIndex&&b.sourceIndex){return a.sourceIndex-b.sourceIndex;}};}})();function tuneContext(context){return query(context,undefined);}
function getElementById(id,context){var doc=context,el;if(context.nodeType!==NodeType.DOCUMENT_NODE){doc=context.ownerDocument;}
el=doc.getElementById(id);if(el&&el.id===id){}
else if(el&&el.parentNode){if(!idEq(el,id)){el=DOM.filter(ANY,'#'+id,context)[0]||null;}
else if(!testByContext(el,context)){el=null;}}else{el=null;}
return el;}
function getElementsByTagName(tag,context){return context&&makeArray(context.getElementsByTagName(tag))||[];}
(function(){var div=doc.createElement('div');div.appendChild(doc.createComment(''));if(div.getElementsByTagName(ANY).length>0){getElementsByTagName=function(tag,context){var ret=makeArray(context.getElementsByTagName(tag));if(tag===ANY){var t=[],i=0,node;while((node=ret[i++])){if(node.nodeType===1){t.push(node);}}
ret=t;}
return ret;};}})();var getElementsByClassName=doc.getElementsByClassName?function(cls,tag,context){if(!context){return[];}
var els=context.getElementsByClassName(cls),ret,i=0,len=els.length,el;if(tag&&tag!==ANY){ret=[];for(;i<len;++i){el=els[i];if(getNodeName(el)==tag){ret.push(el);}}}else{ret=makeArray(els);}
return ret;}:(doc.querySelectorAll?function(cls,tag,context){return context&&makeArray(context.querySelectorAll((tag?tag:'')+'.'+cls))||[];}:function(cls,tag,context){if(!context){return[];}
var els=context.getElementsByTagName(tag||ANY),ret=[],i=0,len=els.length,el;for(;i<len;++i){el=els[i];if(hasClass(el,cls)){ret.push(el);}}
return ret;});function hasClass(el,cls){var className;return(className=el.className)&&(' '+className+' ').indexOf(' '+cls+' ')!==-1;}
function error(msg){S.error('Unsupported selector: '+msg);}
S.mix(DOM,{query:query,get:function(selector,context){return query(selector,context)[0]||null;},unique:unique,filter:function(selector,filter,context){var elems=query(selector,context),sizzle=require('sizzle'),match,tag,id,cls,ret=[];if(isString(filter)&&(filter=trim(filter))&&(match=REG_QUERY.exec(filter))){id=match[1];tag=match[2];cls=match[3];if(!id){filter=function(elem){var tagRe=true,clsRe=true;if(tag){tagRe=getNodeName(elem)==tag;}
if(cls){clsRe=hasClass(elem,cls);}
return clsRe&&tagRe;}}else if(id&&!tag&&!cls){filter=function(elem){return idEq(elem,id);};}}
if(S.isFunction(filter)){ret=S.filter(elems,filter);}
else if(filter&&sizzle){ret=sizzle.matches(filter,elems);}
else{error(filter);}
return ret;},test:function(selector,filter,context){var elements=query(selector,context);return elements.length&&(DOM.filter(elements,filter,context).length===elements.length);}});function idEq(elem,id){var idNode=elem.getAttributeNode('id');return idNode&&idNode.nodeValue===id;}
return DOM;},{requires:['./base']});KISSY.add('dom/style-ie',function(S,DOM,UA,Style){var HUNDRED=100;if(!UA['ie']){return DOM;}
var doc=S.Env.host.document,docElem=doc.documentElement,OPACITY='opacity',STYLE='style',FILTER='filter',CURRENT_STYLE='currentStyle',RUNTIME_STYLE='runtimeStyle',LEFT='left',PX='px',CUSTOM_STYLES=Style._CUSTOM_STYLES,RE_NUM_PX=/^-?\d+(?:px)?$/i,RE_NUM=/^-?\d/,backgroundPosition='backgroundPosition',R_OPACITY=/opacity=([^)]*)/,R_ALPHA=/alpha\([^)]*\)/i;CUSTOM_STYLES[backgroundPosition]={get:function(elem,computed){if(computed){return elem[CURRENT_STYLE][backgroundPosition+'X']+' '+
elem[CURRENT_STYLE][backgroundPosition+'Y'];}else{return elem[STYLE][backgroundPosition];}}};try{if(docElem.style[OPACITY]==null){CUSTOM_STYLES[OPACITY]={get:function(elem,computed){return R_OPACITY.test((computed&&elem[CURRENT_STYLE]?elem[CURRENT_STYLE][FILTER]:elem[STYLE][FILTER])||'')?(parseFloat(RegExp.$1)/HUNDRED)+'':computed?'1':'';},set:function(elem,val){val=parseFloat(val);var style=elem[STYLE],currentStyle=elem[CURRENT_STYLE],opacity=isNaN(val)?'':'alpha('+OPACITY+'='+val*HUNDRED+')',filter=S.trim(currentStyle&&currentStyle[FILTER]||style[FILTER]||'');style.zoom=1;if(val>=1&&S.trim(filter.replace(R_ALPHA,''))===''){style.removeAttribute(FILTER);if(currentStyle&&!currentStyle[FILTER]){return;}}
style.filter=R_ALPHA.test(filter)?filter.replace(R_ALPHA,opacity):filter+(filter?', ':'')+opacity;}};}}
catch(ex){S.log('IE filters ActiveX is disabled. ex = '+ex);}
var IE8=UA['ie']==8,BORDER_MAP={},BORDERS=['','Top','Left','Right','Bottom'];BORDER_MAP['thin']=IE8?'1px':'2px';BORDER_MAP['medium']=IE8?'3px':'4px';BORDER_MAP['thick']=IE8?'5px':'6px';S.each(BORDERS,function(b){var name='border'+b+'Width',styleName='border'+b+'Style';CUSTOM_STYLES[name]={get:function(elem,computed){var currentStyle=computed?elem[CURRENT_STYLE]:0,current=currentStyle&&String(currentStyle[name])||undefined;if(current&&current.indexOf('px')<0){if(BORDER_MAP[current]&&currentStyle[styleName]!=='none'){current=BORDER_MAP[current];}else{current=0;}}
return current;}};});if(!(doc.defaultView||{}).getComputedStyle&&docElem[CURRENT_STYLE]){DOM._getComputedStyle=function(elem,name){name=DOM._cssProps[name]||name;var ret=elem[CURRENT_STYLE]&&elem[CURRENT_STYLE][name];if((!RE_NUM_PX.test(ret)&&RE_NUM.test(ret))&&ret.indexOf(' ')==-1){var style=elem[STYLE],left=style[LEFT],rsLeft=elem[RUNTIME_STYLE]&&elem[RUNTIME_STYLE][LEFT];if(rsLeft){elem[RUNTIME_STYLE][LEFT]=elem[CURRENT_STYLE][LEFT];}
style[LEFT]=name==='fontSize'?'1em':(ret||0);ret=style['pixelLeft']+PX;style[LEFT]=left;if(rsLeft){elem[RUNTIME_STYLE][LEFT]=rsLeft;}}
return ret===''?'auto':ret;};}
return DOM;},{requires:['./base','ua','./style']});KISSY.add('dom/style',function(S,DOM,UA,undefined){var WINDOW=S.Env.host,doc=WINDOW.document,docElem=doc.documentElement,isIE=UA['ie'],STYLE='style',FLOAT='float',CSS_FLOAT='cssFloat',STYLE_FLOAT='styleFloat',WIDTH='width',HEIGHT='height',AUTO='auto',DISPLAY='display',OLD_DISPLAY=DISPLAY+S.now(),NONE='none',myParseInt=parseInt,RE_NUM_PX=/^-?\d+(?:px)?$/i,cssNumber={'fillOpacity':1,'fontWeight':1,'lineHeight':1,'opacity':1,'orphans':1,'widows':1,'zIndex':1,'zoom':1},rmsPrefix=/^-ms-/,RE_DASH=/-([a-z])/ig,CAMEL_CASE_FN=function(all,letter){return letter.toUpperCase();},R_UPPER=/([A-Z]|^ms)/g,EMPTY='',DEFAULT_UNIT='px',CUSTOM_STYLES={},cssProps={},defaultDisplay={};if(docElem[STYLE][CSS_FLOAT]!==undefined){cssProps[FLOAT]=CSS_FLOAT;}
else if(docElem[STYLE][STYLE_FLOAT]!==undefined){cssProps[FLOAT]=STYLE_FLOAT;}
function camelCase(name){return name.replace(rmsPrefix,'ms-').replace(RE_DASH,CAMEL_CASE_FN);}
var defaultDisplayDetectIframe,defaultDisplayDetectIframeDoc;function getDefaultDisplay(tagName){var body,elem;if(!defaultDisplay[tagName]){body=doc.body||doc.documentElement;elem=doc.createElement(tagName);DOM.prepend(elem,body);var oldDisplay=DOM.css(elem,'display');body.removeChild(elem);if(oldDisplay=='none'||oldDisplay==''){if(!defaultDisplayDetectIframe){defaultDisplayDetectIframe=doc.createElement('iframe');defaultDisplayDetectIframe.frameBorder=defaultDisplayDetectIframe.width=defaultDisplayDetectIframe.height=0;DOM.prepend(defaultDisplayDetectIframe,body);var iframeSrc;if(iframeSrc=DOM.getEmptyIframeSrc()){defaultDisplayDetectIframe.src=iframeSrc;}}else{DOM.prepend(defaultDisplayDetectIframe,body);}
if(!defaultDisplayDetectIframeDoc||!defaultDisplayDetectIframe.createElement){try{defaultDisplayDetectIframeDoc=defaultDisplayDetectIframe.contentWindow.document;defaultDisplayDetectIframeDoc.write((doc.compatMode==='CSS1Compat'?'<!doctype html>':'')
+'<html><head>'+
(UA['ie']&&DOM.isCustomDomain()?'<script>'+'document.'+'domain'+"= '"+
doc.domain
+"';</script>":'')
+'</head><body>');defaultDisplayDetectIframeDoc.close();}catch(e){return'block';}}
elem=defaultDisplayDetectIframeDoc.createElement(tagName);defaultDisplayDetectIframeDoc.body.appendChild(elem);oldDisplay=DOM.css(elem,'display');body.removeChild(defaultDisplayDetectIframe);}
defaultDisplay[tagName]=oldDisplay;}
return defaultDisplay[tagName];}
S.mix(DOM,{_camelCase:camelCase,_CUSTOM_STYLES:CUSTOM_STYLES,_cssProps:cssProps,_getComputedStyle:function(elem,name){var val='',computedStyle,d=elem.ownerDocument;name=name.replace(R_UPPER,'-$1').toLowerCase();if(computedStyle=d.defaultView.getComputedStyle(elem,null)){val=computedStyle.getPropertyValue(name)||computedStyle[name];}
if(val==''&&!DOM.contains(d.documentElement,elem)){name=cssProps[name]||name;val=elem[STYLE][name];}
return val;},style:function(selector,name,val){var els=DOM.query(selector),elem=els[0],i;if(S.isPlainObject(name)){for(var k in name){if(name.hasOwnProperty(k)){for(i=els.length-1;i>=0;i--){style(els[i],k,name[k]);}}}
return undefined;}
if(val===undefined){var ret='';if(elem){ret=style(elem,name,val);}
return ret;}else{for(i=els.length-1;i>=0;i--){style(els[i],name,val);}}
return undefined;},css:function(selector,name,val){var els=DOM.query(selector),elem=els[0],i;if(S.isPlainObject(name)){for(var k in name){if(name.hasOwnProperty(k)){for(i=els.length-1;i>=0;i--){style(els[i],k,name[k]);}}}
return undefined;}
name=camelCase(name);var hook=CUSTOM_STYLES[name];if(val===undefined){var ret='';if(elem){if(hook&&'get'in hook&&(ret=hook.get(elem,true))!==undefined){}else{ret=DOM._getComputedStyle(elem,name);}}
return ret===undefined?'':ret;}
else{for(i=els.length-1;i>=0;i--){style(els[i],name,val);}}
return undefined;},show:function(selector){var els=DOM.query(selector),elem,i;for(i=els.length-1;i>=0;i--){elem=els[i];elem[STYLE][DISPLAY]=DOM.data(elem,OLD_DISPLAY)||EMPTY;if(DOM.css(elem,DISPLAY)===NONE){var tagName=elem.tagName.toLowerCase(),old=getDefaultDisplay(tagName);DOM.data(elem,OLD_DISPLAY,old);elem[STYLE][DISPLAY]=old;}}},hide:function(selector){var els=DOM.query(selector),elem,i;for(i=els.length-1;i>=0;i--){elem=els[i];var style=elem[STYLE],old=style[DISPLAY];if(old!==NONE){if(old){DOM.data(elem,OLD_DISPLAY,old);}
style[DISPLAY]=NONE;}}},toggle:function(selector){var els=DOM.query(selector),elem,i;for(i=els.length-1;i>=0;i--){elem=els[i];if(DOM.css(elem,DISPLAY)===NONE){DOM.show(elem);}else{DOM.hide(elem);}}},addStyleSheet:function(refWin,cssText,id){refWin=refWin||WINDOW;if(S.isString(refWin)){id=cssText;cssText=refWin;refWin=WINDOW;}
refWin=DOM.get(refWin);var win=DOM._getWin(refWin),doc=win.document,elem;if(id&&(id=id.replace('#',EMPTY))){elem=DOM.get('#'+id,doc);}
if(elem){return;}
elem=DOM.create('<style>',{id:id},doc);DOM.get('head',doc).appendChild(elem);if(elem.styleSheet){elem.styleSheet.cssText=cssText;}else{elem.appendChild(doc.createTextNode(cssText));}},unselectable:function(selector){var _els=DOM.query(selector),elem,j;for(j=_els.length-1;j>=0;j--){elem=_els[j];if(UA['gecko']){elem[STYLE]['MozUserSelect']='none';}
else if(UA['webkit']){elem[STYLE]['KhtmlUserSelect']='none';}else{if(UA['ie']||UA['opera']){var e,i=0,els=elem.getElementsByTagName('*');elem.setAttribute('unselectable','on');while((e=els[i++])){switch(e.tagName.toLowerCase()){case'iframe':case'textarea':case'input':case'select':break;default:e.setAttribute('unselectable','on');}}}}}},innerWidth:0,innerHeight:0,outerWidth:0,outerHeight:0,width:0,height:0});function capital(str){return str.charAt(0).toUpperCase()+str.substring(1);}
S.each([WIDTH,HEIGHT],function(name){DOM['inner'+capital(name)]=function(selector){var el=DOM.get(selector);return el&&getWHIgnoreDisplay(el,name,'padding');};DOM['outer'+capital(name)]=function(selector,includeMargin){var el=DOM.get(selector);return el&&getWHIgnoreDisplay(el,name,includeMargin?'margin':'border');};DOM[name]=function(selector,val){var ret=DOM.css(selector,name,val);if(ret){ret=parseFloat(ret);}
return ret;};});var cssShow={position:'absolute',visibility:'hidden',display:'block'};S.each(['height','width'],function(name){CUSTOM_STYLES[name]={get:function(elem,computed){if(computed){return getWHIgnoreDisplay(elem,name)+'px';}},set:function(elem,value){if(RE_NUM_PX.test(value)){value=parseFloat(value);if(value>=0){return value+'px';}}else{return value;}}};});S.each(['left','top'],function(name){CUSTOM_STYLES[name]={get:function(elem,computed){if(computed){var val=DOM._getComputedStyle(elem,name),offset;if(val===AUTO){val=0;if(S.inArray(DOM.css(elem,'position'),['absolute','fixed'])){offset=elem[name==='left'?'offsetLeft':'offsetTop'];if(isIE&&doc['documentMode']!=9||UA['opera']){offset-=elem.offsetParent&&elem.offsetParent['client'+(name=='left'?'Left':'Top')]||0;}
val=offset-(myParseInt(DOM.css(elem,'margin-'+name))||0);}
val+='px';}
return val;}}};});function swap(elem,options,callback){var old={};for(var name in options){if(options.hasOwnProperty(name)){old[name]=elem[STYLE][name];elem[STYLE][name]=options[name];}}
callback.call(elem);for(name in options){if(options.hasOwnProperty(name)){elem[STYLE][name]=old[name];}}}
function style(elem,name,val){var style;if(elem.nodeType===3||elem.nodeType===8||!(style=elem[STYLE])){return undefined;}
name=camelCase(name);var ret,hook=CUSTOM_STYLES[name];name=cssProps[name]||name;if(val!==undefined){if(val===null||val===EMPTY){val=EMPTY;}
else if(!isNaN(Number(val))&&!cssNumber[name]){val+=DEFAULT_UNIT;}
if(hook&&hook.set){val=hook.set(elem,val);}
if(val!==undefined){try{style[name]=val;}catch(e){S.log('css set error :'+e);}
if(val===EMPTY&&style.removeAttribute){style.removeAttribute(name);}}
if(!style.cssText){elem.removeAttribute('style');}
return undefined;}
else{if(hook&&'get'in hook&&(ret=hook.get(elem,false))!==undefined){}else{ret=style[name];}
return ret===undefined?'':ret;}}
function getWHIgnoreDisplay(elem){var val,args=arguments;if(elem.offsetWidth!==0){val=getWH.apply(undefined,args);}else{swap(elem,cssShow,function(){val=getWH.apply(undefined,args);});}
return val;}
function getWH(elem,name,extra){if(S.isWindow(elem)){return name==WIDTH?DOM.viewportWidth(elem):DOM.viewportHeight(elem);}else if(elem.nodeType==9){return name==WIDTH?DOM.docWidth(elem):DOM.docHeight(elem);}
var which=name===WIDTH?['Left','Right']:['Top','Bottom'],val=name===WIDTH?elem.offsetWidth:elem.offsetHeight;if(val>0){if(extra!=='border'){S.each(which,function(w){if(!extra){val-=parseFloat(DOM.css(elem,'padding'+w))||0;}
if(extra==='margin'){val+=parseFloat(DOM.css(elem,extra+w))||0;}else{val-=parseFloat(DOM.css(elem,'border'+w+'Width'))||0;}});}
return val;}
val=DOM._getComputedStyle(elem,name);if(val==null||(Number(val))<0){val=elem.style[name]||0;}
val=parseFloat(val)||0;if(extra){S.each(which,function(w){val+=parseFloat(DOM.css(elem,'padding'+w))||0;if(extra!=='padding'){val+=parseFloat(DOM.css(elem,'border'+w+'Width'))||0;}
if(extra==='margin'){val+=parseFloat(DOM.css(elem,extra+w))||0;}});}
return val;}
return DOM;},{requires:['dom/base','ua']});KISSY.add('dom/traversal',function(S,DOM,undefined){var doc=S.Env.host.document,NodeType=DOM.NodeType,documentElement=doc.documentElement,CONTAIN_MASK=16,__contains=documentElement.compareDocumentPosition?function(a,b){return!!(a.compareDocumentPosition(b)&CONTAIN_MASK);}:documentElement.contains?function(a,b){if(a.nodeType==NodeType.DOCUMENT_NODE){a=a.documentElement;}
b=b.parentNode;if(a==b){return true;}
if(b&&b.nodeType==NodeType.ELEMENT_NODE){return a.contains&&a.contains(b);}else{return false;}}:0;S.mix(DOM,{closest:function(selector,filter,context,allowTextNode){return nth(selector,filter,'parentNode',function(elem){return elem.nodeType!=NodeType.DOCUMENT_FRAGMENT_NODE;},context,true,allowTextNode);},parent:function(selector,filter,context){return nth(selector,filter,'parentNode',function(elem){return elem.nodeType!=NodeType.DOCUMENT_FRAGMENT_NODE;},context,undefined);},first:function(selector,filter,allowTextNode){var elem=DOM.get(selector);return nth(elem&&elem.firstChild,filter,'nextSibling',undefined,undefined,true,allowTextNode);},last:function(selector,filter,allowTextNode){var elem=DOM.get(selector);return nth(elem&&elem.lastChild,filter,'previousSibling',undefined,undefined,true,allowTextNode);},next:function(selector,filter,allowTextNode){return nth(selector,filter,'nextSibling',undefined,undefined,undefined,allowTextNode);},prev:function(selector,filter,allowTextNode){return nth(selector,filter,'previousSibling',undefined,undefined,undefined,allowTextNode);},siblings:function(selector,filter,allowTextNode){return getSiblings(selector,filter,true,allowTextNode);},children:function(selector,filter){return getSiblings(selector,filter,undefined);},contents:function(selector,filter){return getSiblings(selector,filter,undefined,1);},contains:function(container,contained){container=DOM.get(container);contained=DOM.get(contained);if(container&&contained){return __contains(container,contained);}
return false;},equals:function(n1,n2){n1=DOM.query(n1);n2=DOM.query(n2);if(n1.length!=n2.length){return false;}
for(var i=n1.length;i>=0;i--){if(n1[i]!=n2[i]){return false;}}
return true;}});function nth(elem,filter,direction,extraFilter,context,includeSef,allowTextNode){if(!(elem=DOM.get(elem))){return null;}
if(filter===0){return elem;}
if(!includeSef){elem=elem[direction];}
if(!elem){return null;}
context=(context&&DOM.get(context))||null;if(filter===undefined){filter=1;}
var ret=[],isArray=S.isArray(filter),fi,flen;if(S.isNumber(filter)){fi=0;flen=filter;filter=function(){return++fi===flen;};}
while(elem&&elem!=context){if((elem.nodeType==NodeType.ELEMENT_NODE||elem.nodeType==NodeType.TEXT_NODE&&allowTextNode)&&testFilter(elem,filter)&&(!extraFilter||extraFilter(elem))){ret.push(elem);if(!isArray){break;}}
elem=elem[direction];}
return isArray?ret:ret[0]||null;}
function testFilter(elem,filter){if(!filter){return true;}
if(S.isArray(filter)){for(var i=0;i<filter.length;i++){if(DOM.test(elem,filter[i])){return true;}}}else if(DOM.test(elem,filter)){return true;}
return false;}
function getSiblings(selector,filter,parent,allowText){var ret=[],tmp,i,el,elem=DOM.get(selector),parentNode=elem;if(elem&&parent){parentNode=elem.parentNode;}
if(parentNode){tmp=S.makeArray(parentNode.childNodes);for(i=0;i<tmp.length;i++){el=tmp[i];if(!allowText&&el.nodeType!=NodeType.ELEMENT_NODE){continue;}
if(el==elem){continue;}
ret.push(el);}
if(filter){ret=DOM.filter(ret,filter);}}
return ret;}
return DOM;},{requires:['./base']});KISSY.add('event/add',function(S,Event,DOM,Utils,EventObject,handle,_data,specials){var simpleAdd=Utils.simpleAdd,isValidTarget=Utils.isValidTarget,isIdenticalHandler=Utils.isIdenticalHandler;function addDomEvent(target,type,eventHandler,handlers,handleObj){var special=specials[type]||{};if(!handlers.length&&(!special.setup||special.setup.call(target)===false)){simpleAdd(target,type,eventHandler)}
if(special.add){special.add.call(target,handleObj);}}
S.mix(Event,{__add:function(isNativeTarget,target,type,fn,scope){var typedGroups=Utils.getTypedGroups(type);type=typedGroups[0];var groups=typedGroups[1],isCustomEvent=!isNativeTarget,eventDesc,data,s=isNativeTarget&&specials[type],originalType,last,selector;if(S.isObject(fn)){last=fn.last;scope=fn.scope;data=fn.data;selector=fn.selector;originalType=fn.originalType;fn=fn.fn;if(selector&&!originalType){if(s&&s['delegateFix']){originalType=type;type=s['delegateFix'];}}}
if(!selector&&!originalType){if(s&&s['onFix']){originalType=type;type=s['onFix'];}}
if(!type||!target||!S.isFunction(fn)||(isNativeTarget&&!isValidTarget(target))){return;}
eventDesc=_data._data(target,undefined,isCustomEvent);if(!eventDesc){_data._data(target,eventDesc={},isCustomEvent);}
var events=eventDesc.events=eventDesc.events||{},handlers=events[type]=events[type]||[],handleObj={fn:fn,scope:scope,selector:selector,last:last,data:data,groups:groups,originalType:originalType},eventHandler=eventDesc.handler;if(isNativeTarget&&!eventHandler){eventHandler=eventDesc.handler=function(event,data){if(typeof KISSY=='undefined'||event&&event.type==Utils.Event_Triggered){return;}
var currentTarget=eventHandler.target,type;if(!event||!event.fixed){event=new EventObject(currentTarget,event);}
type=event.type;if(S.isPlainObject(data)){S.mix(event,data);}
if(type){event.type=type;}
return handle(currentTarget,event);};eventHandler.target=target;}
for(var i=handlers.length-1;i>=0;--i){if(isIdenticalHandler(handlers[i],handleObj,target)){return;}}
if(isNativeTarget){addDomEvent(target,type,eventHandler,handlers,handleObj);target=null;}
if(selector){var delegateIndex=handlers.delegateCount=handlers.delegateCount||0;handlers.splice(delegateIndex,0,handleObj);handlers.delegateCount++;}else{handlers.lastCount=handlers.lastCount||0;if(last){handlers.push(handleObj);handlers.lastCount++;}else{handlers.splice(handlers.length-handlers.lastCount,0,handleObj);}}},add:function(targets,type,fn,scope){type=S.trim(type);if(Utils.batchForType(Event.add,targets,type,fn,scope)){return targets;}
targets=DOM.query(targets);for(var i=targets.length-1;i>=0;i--){Event.__add(true,targets[i],type,fn,scope);}
return targets;}});},{requires:['./base','dom','./utils','./object','./handle','./data','./special']});KISSY.add('event/base',function(S,DOM,EventObject,Utils,handle,_data,special){var isValidTarget=Utils.isValidTarget,splitAndRun=Utils.splitAndRun,getNodeName=DOM.nodeName,trim=S.trim,TRIGGERED_NONE=Utils.TRIGGERED_NONE;var Event={_clone:function(src,dest){if(!isValidTarget(dest)||!isValidTarget(src)||!_data._hasData(src,false)){return;}
var eventDesc=_data._data(src,undefined,false),events=eventDesc.events;S.each(events,function(handlers,type){S.each(handlers,function(handler){Event.on(dest,type,{data:handler.data,fn:handler.fn,groups:handler.groups,last:handler.last,originalType:handler.originalType,scope:handler.scope,selector:handler.selector});});});},fire:function(targets,eventType,eventData,onlyHandlers){var ret=true,r;eventData=eventData||{};if(S.isString(eventType)){eventType=trim(eventType);if(eventType.indexOf(' ')>-1){splitAndRun(eventType,function(t){r=Event.fire(targets,t,eventData,onlyHandlers);if(ret!==false){ret=r;}});return ret;}
eventData.type=eventType;}else if(eventType instanceof EventObject){eventData=eventType;eventType=eventData.type;}
var typedGroups=Utils.getTypedGroups(eventType),_ks_groups=typedGroups[1];if(_ks_groups){_ks_groups=Utils.getGroupsRe(_ks_groups);}
eventType=typedGroups[0];S.mix(eventData,{type:eventType,_ks_groups:_ks_groups});targets=DOM.query(targets);for(var i=targets.length-1;i>=0;i--){r=fireDOMEvent(targets[i],eventType,eventData,onlyHandlers);if(ret!==false){ret=r;}}
return ret;},fireHandler:function(targets,eventType,eventData){return Event.fire(targets,eventType,eventData,1);}};function fireDOMEvent(target,eventType,eventData,onlyHandlers){if(!isValidTarget(target)){return false;}
var s=special[eventType];if(s&&s['onFix']){eventType=s['onFix'];}
var event,ret=true;if(eventData instanceof EventObject){event=eventData;}else{event=new EventObject(target,undefined,eventType);S.mix(event,eventData);}
event._ks_fired=1;event.type=eventType;var cur=target,t,win=DOM._getWin(cur.ownerDocument||cur),ontype='on'+eventType;do{event.currentTarget=cur;t=handle(cur,event);if(ret!==false){ret=t;}
if(cur[ontype]&&cur[ontype].call(cur)===false){event.preventDefault();}
cur=cur.parentNode||cur.ownerDocument||(cur===target.ownerDocument)&&win;}while(!onlyHandlers&&cur&&!event.isPropagationStopped);if(!onlyHandlers&&!event.isDefaultPrevented){if(!(eventType==='click'&&getNodeName(target)=='a')){var old;try{if(ontype&&target[eventType]&&((eventType!=='focus'&&eventType!=='blur')||target.offsetWidth!==0)&&!S.isWindow(target)){old=target[ontype];if(old){target[ontype]=null;}
Utils.Event_Triggered=eventType;target[eventType]();}}catch(ieError){S.log('trigger action error : ');S.log(ieError);}
if(old){target[ontype]=old;}
Utils.Event_Triggered=TRIGGERED_NONE;}}
return ret;}
return Event;},{requires:['dom','./object','./utils','./handle','./data','./special']});KISSY.add('event/change',function(S,UA,Event,DOM,special){var mode=S.Env.host.document['documentMode'];if(UA['ie']&&(UA['ie']<9||(mode&&mode<9))){var R_FORM_EL=/^(?:textarea|input|select)$/i;function isFormElement(n){return R_FORM_EL.test(n.nodeName);}
function isCheckBoxOrRadio(el){var type=el.type;return type=='checkbox'||type=='radio';}
special['change']={setup:function(){var el=this;if(isFormElement(el)){if(isCheckBoxOrRadio(el)){Event.on(el,'propertychange',propertyChange);Event.on(el,'click',onClick);}else{return false;}}else{Event.on(el,'beforeactivate',beforeActivate);}},tearDown:function(){var el=this;if(isFormElement(el)){if(isCheckBoxOrRadio(el)){Event.remove(el,'propertychange',propertyChange);Event.remove(el,'click',onClick);}else{return false;}}else{Event.remove(el,'beforeactivate',beforeActivate);S.each(DOM.query('textarea,input,select',el),function(fel){if(fel.__changeHandler){fel.__changeHandler=0;Event.remove(fel,'change',{fn:changeHandler,last:1});}});}}};function propertyChange(e){if(e.originalEvent.propertyName=='checked'){this.__changed=1;}}
function onClick(e){if(this.__changed){this.__changed=0;Event.fire(this,'change',e);}}
function beforeActivate(e){var t=e.target;if(isFormElement(t)&&!t.__changeHandler){t.__changeHandler=1;Event.on(t,'change',{fn:changeHandler,last:1});}}
function changeHandler(e){var fel=this;if(e.isPropagationStopped||isCheckBoxOrRadio(fel)){return;}
var p;if(p=fel.parentNode){Event.fire(p,'change',e);}}}},{requires:['ua','./base','dom','./special']});KISSY.add('event/data',function(S,DOM,Utils){var EVENT_GUID=Utils.EVENT_GUID,data;data={_hasData:function(elem,isCustomEvent){if(isCustomEvent){return elem[EVENT_GUID]&&(!S.isEmptyObject(elem[EVENT_GUID]));}else{return DOM.hasData(elem,EVENT_GUID);}},_data:function(elem,v,isCustomEvent){if(isCustomEvent){if(v!==undefined){return elem[EVENT_GUID]=v;}else{return elem[EVENT_GUID];}}else{return DOM.data(elem,EVENT_GUID,v);}},_removeData:function(elem,isCustomEvent){if(isCustomEvent){delete elem[EVENT_GUID];}else{return DOM.removeData(elem,EVENT_GUID);}}};return data;},{requires:['dom','./utils']});KISSY.add('event',function(S,_data,KeyCodes,Event,Target,Object){S.mix(Event,{KeyCodes:KeyCodes,Target:Target,Object:Object,on:Event.add,detach:Event.remove,delegate:function(targets,eventType,selector,fn,scope){return Event.add(targets,eventType,{fn:fn,scope:scope,selector:selector});},undelegate:function(targets,eventType,selector,fn,scope){return Event.remove(targets,eventType,{fn:fn,scope:scope,selector:selector});}});S.mix(Event,_data);S.mix(S,{Event:Event,EventTarget:Event.Target,EventObject:Event.Object});return Event;},{requires:['event/data','event/key-codes','event/base','event/target','event/object','event/focusin','event/hashchange','event/valuechange','event/mouseenter','event/submit','event/change','event/mousewheel','event/add','event/remove']});KISSY.add('event/focusin',function(S,UA,Event,special){if(!UA['ie']){S.each([{name:'focusin',fix:'focus'},{name:'focusout',fix:'blur'}],function(o){var key=S.guid('attaches_'+S.now()+'_');special[o.name]={setup:function(){var doc=this.ownerDocument||this;if(!(key in doc)){doc[key]=0;}
doc[key]+=1;if(doc[key]===1){doc.addEventListener(o.fix,handler,true);}},tearDown:function(){var doc=this.ownerDocument||this;doc[key]-=1;if(doc[key]===0){doc.removeEventListener(o.fix,handler,true);}}};function handler(event){var target=event.target;return Event.fire(target,o.name);}});}
special['focus']={delegateFix:'focusin'};special['blur']={delegateFix:'focusout'};return Event;},{requires:['ua','./base','./special']});KISSY.add('event/handle',function(S,DOM,_data,special){function getEvents(target,isCustomEvent){var eventDesc=_data._data(target,undefined,isCustomEvent);return eventDesc&&eventDesc.events;}
function getHandlers(target,type,isCustomEvent){var events=getEvents(target,isCustomEvent)||{};return events[type]||[];}
return function(currentTarget,event,isCustomEvent){var handlers=getHandlers(currentTarget,event.type,isCustomEvent),target=event.target,currentTarget0,allHandlers=[],ret,gRet,handlerObj,i,j,delegateCount=handlers.delegateCount||0,len,currentTargetHandlers,currentTargetHandler,handler;if(delegateCount&&!target.disabled){while(target!=currentTarget){currentTargetHandlers=[];for(i=0;i<delegateCount;i++){handler=handlers[i];if(DOM.test(target,handler.selector)){currentTargetHandlers.push(handler);}}
if(currentTargetHandlers.length){allHandlers.push({currentTarget:target,'currentTargetHandlers':currentTargetHandlers});}
target=target.parentNode||currentTarget;}}
allHandlers.push({currentTarget:currentTarget,currentTargetHandlers:handlers.slice(delegateCount)});var eventType=event.type,s,t,_ks_groups=event._ks_groups;for(i=0,len=allHandlers.length;!event.isPropagationStopped&&i<len;++i){handlerObj=allHandlers[i];currentTargetHandlers=handlerObj.currentTargetHandlers;currentTarget0=handlerObj.currentTarget;event.currentTarget=currentTarget0;for(j=0;!event.isImmediatePropagationStopped&&j<currentTargetHandlers.length;j++){currentTargetHandler=currentTargetHandlers[j];if(_ks_groups&&(!currentTargetHandler.groups||!(currentTargetHandler.groups.match(_ks_groups)))){continue;}
var data=currentTargetHandler.data;event.type=currentTargetHandler.originalType||eventType;if(!isCustomEvent&&(s=special[event.type])&&s.handle){t=s.handle(event,currentTargetHandler,data);if(t.length>0){ret=t[0];}}else{ret=currentTargetHandler.fn.call(currentTargetHandler.scope||currentTarget,event,data);}
if(gRet!==false){gRet=ret;}
if(ret===false){event.halt();}}}
return gRet;}},{requires:['dom','./data','./special']});KISSY.add('event/hashchange',function(S,Event,DOM,UA,special){var win=S.Env.host,doc=win.document,docMode=doc['documentMode'],ie=docMode||UA['ie'],HASH_CHANGE='hashchange';if((!('on'+HASH_CHANGE in win))||ie&&ie<8){function getIframeDoc(iframe){return iframe.contentWindow.document;}
var POLL_INTERVAL=50,IFRAME_TEMPLATE='<html><head><title>'+(doc.title||'')+' - {hash}</title>{head}</head><body>{hash}</body></html>',getHash=function(){var uri=new S.Uri(location.href);return'#'+uri.getFragment();},timer,lastHash,poll=function(){var hash=getHash();if(hash!==lastHash){lastHash=hash;hashChange(hash);}
timer=setTimeout(poll,POLL_INTERVAL);},hashChange=ie&&ie<8?function(hash){var html=S.substitute(IFRAME_TEMPLATE,{hash:S.escapeHTML(hash),head:DOM.isCustomDomain()?("<script>"+"document."+"domain = '"+
doc.domain
+"';</script>"):''}),iframeDoc=getIframeDoc(iframe);try{iframeDoc.open();iframeDoc.write(html);iframeDoc.close();}catch(e){}}:function(){notifyHashChange();},notifyHashChange=function(){Event.fire(win,HASH_CHANGE);},setup=function(){if(!timer){poll();}},tearDown=function(){timer&&clearTimeout(timer);timer=0;},iframe;if(ie<8){setup=function(){if(!iframe){var iframeSrc=DOM.getEmptyIframeSrc();iframe=DOM.create('<iframe '+
(iframeSrc?'src="'+iframeSrc+'"':'')+' style="display: none" '+'height="0" '+'width="0" '+'tabindex="-1" '+'title="empty"/>');DOM.prepend(iframe,doc.documentElement);Event.add(iframe,'load',function(){Event.remove(iframe,'load');hashChange(getHash());Event.add(iframe,'load',onIframeLoad);poll();});doc.onpropertychange=function(){try{if(event.propertyName==='title'){getIframeDoc(iframe).title=doc.title+' - '+getHash();}}catch(e){}};function onIframeLoad(){var c=S.trim(getIframeDoc(iframe).body.innerText),ch=getHash();if(c!=ch){S.log('set loc hash :'+c);location.hash=c;lastHash=c;}
notifyHashChange();}}};tearDown=function(){timer&&clearTimeout(timer);timer=0;Event.detach(iframe);DOM.remove(iframe);iframe=0;};}
special[HASH_CHANGE]={setup:function(){if(this!==win){return;}
lastHash=getHash();setup();},tearDown:function(){if(this!==win){return;}
tearDown();}};}},{requires:['./base','dom','ua','./special']});KISSY.add('event/key-codes',function(S,UA){var KeyCodes={MAC_ENTER:3,BACKSPACE:8,TAB:9,NUM_CENTER:12,ENTER:13,SHIFT:16,CTRL:17,ALT:18,PAUSE:19,CAPS_LOCK:20,ESC:27,SPACE:32,PAGE_UP:33,PAGE_DOWN:34,END:35,HOME:36,LEFT:37,UP:38,RIGHT:39,DOWN:40,PRINT_SCREEN:44,INSERT:45,DELETE:46,ZERO:48,ONE:49,TWO:50,THREE:51,FOUR:52,FIVE:53,SIX:54,SEVEN:55,EIGHT:56,NINE:57,QUESTION_MARK:63,A:65,B:66,C:67,D:68,E:69,F:70,G:71,H:72,I:73,J:74,K:75,L:76,M:77,N:78,O:79,P:80,Q:81,R:82,S:83,T:84,U:85,V:86,W:87,X:88,Y:89,Z:90,META:91,WIN_KEY_RIGHT:92,CONTEXT_MENU:93,NUM_ZERO:96,NUM_ONE:97,NUM_TWO:98,NUM_THREE:99,NUM_FOUR:100,NUM_FIVE:101,NUM_SIX:102,NUM_SEVEN:103,NUM_EIGHT:104,NUM_NINE:105,NUM_MULTIPLY:106,NUM_PLUS:107,NUM_MINUS:109,NUM_PERIOD:110,NUM_DIVISION:111,F1:112,F2:113,F3:114,F4:115,F5:116,F6:117,F7:118,F8:119,F9:120,F10:121,F11:122,F12:123,NUMLOCK:144,SEMICOLON:186,DASH:189,EQUALS:187,COMMA:188,PERIOD:190,SLASH:191,APOSTROPHE:192,SINGLE_QUOTE:222,OPEN_SQUARE_BRACKET:219,BACKSLASH:220,CLOSE_SQUARE_BRACKET:221,WIN_KEY:224,MAC_FF_META:224,WIN_IME:229};KeyCodes.isTextModifyingKeyEvent=function(e){if(e.altKey&&!e.ctrlKey||e.metaKey||e.keyCode>=KeyCodes.F1&&e.keyCode<=KeyCodes.F12){return false;}
switch(e.keyCode){case KeyCodes.ALT:case KeyCodes.CAPS_LOCK:case KeyCodes.CONTEXT_MENU:case KeyCodes.CTRL:case KeyCodes.DOWN:case KeyCodes.END:case KeyCodes.ESC:case KeyCodes.HOME:case KeyCodes.INSERT:case KeyCodes.LEFT:case KeyCodes.MAC_FF_META:case KeyCodes.META:case KeyCodes.NUMLOCK:case KeyCodes.NUM_CENTER:case KeyCodes.PAGE_DOWN:case KeyCodes.PAGE_UP:case KeyCodes.PAUSE:case KeyCodes.PRINT_SCREEN:case KeyCodes.RIGHT:case KeyCodes.SHIFT:case KeyCodes.UP:case KeyCodes.WIN_KEY:case KeyCodes.WIN_KEY_RIGHT:return false;default:return true;}};KeyCodes.isCharacterKey=function(keyCode){if(keyCode>=KeyCodes.ZERO&&keyCode<=KeyCodes.NINE){return true;}
if(keyCode>=KeyCodes.NUM_ZERO&&keyCode<=KeyCodes.NUM_MULTIPLY){return true;}
if(keyCode>=KeyCodes.A&&keyCode<=KeyCodes.Z){return true;}
if(UA.webkit&&keyCode==0){return true;}
switch(keyCode){case KeyCodes.SPACE:case KeyCodes.QUESTION_MARK:case KeyCodes.NUM_PLUS:case KeyCodes.NUM_MINUS:case KeyCodes.NUM_PERIOD:case KeyCodes.NUM_DIVISION:case KeyCodes.SEMICOLON:case KeyCodes.DASH:case KeyCodes.EQUALS:case KeyCodes.COMMA:case KeyCodes.PERIOD:case KeyCodes.SLASH:case KeyCodes.APOSTROPHE:case KeyCodes.SINGLE_QUOTE:case KeyCodes.OPEN_SQUARE_BRACKET:case KeyCodes.BACKSLASH:case KeyCodes.CLOSE_SQUARE_BRACKET:return true;default:return false;}};return KeyCodes;},{requires:['ua']});KISSY.add('event/mouseenter',function(S,Event,DOM,UA,special){S.each([{name:'mouseenter',fix:'mouseover'},{name:'mouseleave',fix:'mouseout'}],function(o){special[o.name]={onFix:o.fix,delegateFix:o.fix,handle:function(event,handler,data){var currentTarget=event.currentTarget,relatedTarget=event.relatedTarget;if(!relatedTarget||(relatedTarget!==currentTarget&&!DOM.contains(currentTarget,relatedTarget))){return[handler.fn.call(handler.scope||currentTarget,event,data)];}
return[];}};});return Event;},{requires:['./base','dom','ua','./special']});KISSY.add('event/mousewheel',function(S,Event,UA,Utils,EventObject,handle,_data,special){var MOUSE_WHEEL=UA.gecko?'DOMMouseScroll':'mousewheel',simpleRemove=Utils.simpleRemove,simpleAdd=Utils.simpleAdd,MOUSE_WHEEL_HANDLER='mousewheelHandler';function handler(e){var deltaX,currentTarget=this,deltaY,delta,detail=e.detail;if(e.wheelDelta){delta=e.wheelDelta/120;}
if(e.detail){delta=-(detail%3==0?detail/3:detail);}
if(e.axis!==undefined){if(e.axis===e['HORIZONTAL_AXIS']){deltaY=0;deltaX=-1*delta;}else if(e.axis===e['VERTICAL_AXIS']){deltaX=0;deltaY=delta;}}
if(e['wheelDeltaY']!==undefined){deltaY=e['wheelDeltaY']/120;}
if(e['wheelDeltaX']!==undefined){deltaX=-1*e['wheelDeltaX']/120;}
if(!deltaX&&!deltaY){deltaY=delta;}
e=new EventObject(currentTarget,e);S.mix(e,{deltaY:deltaY,delta:delta,deltaX:deltaX,type:'mousewheel'});return handle(currentTarget,e);}
special['mousewheel']={setup:function(){var el=this,mousewheelHandler,eventDesc=_data._data(el,undefined,false);mousewheelHandler=eventDesc[MOUSE_WHEEL_HANDLER]=S.bind(handler,el);simpleAdd(this,MOUSE_WHEEL,mousewheelHandler);},tearDown:function(){var el=this,mousewheelHandler,eventDesc=_data._data(el,undefined,false);mousewheelHandler=eventDesc[MOUSE_WHEEL_HANDLER];simpleRemove(this,MOUSE_WHEEL,mousewheelHandler);delete eventDesc[mousewheelHandler];}};},{requires:['./base','ua','./utils','./object','./handle','./data','./special']});KISSY.add('event/object',function(S,undefined){var doc=S.Env.host.document,TRUE=true,FALSE=false,props=('altKey attrChange attrName bubbles button cancelable '+'charCode clientX clientY ctrlKey currentTarget data detail '+'eventPhase fromElement handler keyCode metaKey '+'newValue offsetX offsetY originalTarget pageX pageY prevValue '+'relatedNode relatedTarget screenX screenY shiftKey srcElement '+'target toElement view wheelDelta which axis').split(' ');function EventObject(currentTarget,domEvent,type){var self=this;self.originalEvent=domEvent||{};self.currentTarget=currentTarget;if(domEvent){self.type=domEvent.type;self.isDefaultPrevented=(domEvent['defaultPrevented']||domEvent.returnValue===FALSE||domEvent['getPreventDefault']&&domEvent['getPreventDefault']())?TRUE:FALSE;self._fix();}
else{self.type=type;self.target=currentTarget;}
self.currentTarget=currentTarget;self.fixed=TRUE;}
EventObject.prototype={constructor:EventObject,isDefaultPrevented:FALSE,isPropagationStopped:FALSE,isImmediatePropagationStopped:FALSE,_fix:function(){var self=this,originalEvent=self.originalEvent,l=props.length,prop,ct=self.currentTarget,ownerDoc=(ct.nodeType===9)?ct:(ct.ownerDocument||doc);while(l){prop=props[--l];self[prop]=originalEvent[prop];}
if(!self.target){self.target=self.srcElement||ownerDoc;}
if(self.target.nodeType===3){self.target=self.target.parentNode;}
if(!self.relatedTarget&&self.fromElement){self.relatedTarget=(self.fromElement===self.target)?self.toElement:self.fromElement;}
if(self.pageX===undefined&&self.clientX!==undefined){var docEl=ownerDoc.documentElement,bd=ownerDoc.body;self.pageX=self.clientX+(docEl&&docEl.scrollLeft||bd&&bd.scrollLeft||0)-(docEl&&docEl.clientLeft||bd&&bd.clientLeft||0);self.pageY=self.clientY+(docEl&&docEl.scrollTop||bd&&bd.scrollTop||0)-(docEl&&docEl.clientTop||bd&&bd.clientTop||0);}
if(self.which===undefined){self.which=(self.charCode===undefined)?self.keyCode:self.charCode;}
if(self.metaKey===undefined){self.metaKey=self.ctrlKey;}
if(!self.which&&self.button!==undefined){self.which=(self.button&1?1:(self.button&2?3:(self.button&4?2:0)));}},preventDefault:function(){var e=this.originalEvent;if(e.preventDefault){e.preventDefault();}
else{e.returnValue=FALSE;}
this.isDefaultPrevented=TRUE;},stopPropagation:function(){var e=this.originalEvent;if(e.stopPropagation){e.stopPropagation();}
else{e.cancelBubble=TRUE;}
this.isPropagationStopped=TRUE;},stopImmediatePropagation:function(){var self=this;self.isImmediatePropagationStopped=TRUE;self.stopPropagation();},halt:function(immediate){var self=this;if(immediate){self.stopImmediatePropagation();}else{self.stopPropagation();}
self.preventDefault();}};return EventObject;});KISSY.add('event/remove',function(S,Event,DOM,Utils,_data,EVENT_SPECIAL){var isValidTarget=Utils.isValidTarget,simpleRemove=Utils.simpleRemove;S.mix(Event,{__remove:function(isNativeTarget,target,type,fn,scope){if(!target||(isNativeTarget&&!isValidTarget(target))){return;}
var typedGroups=Utils.getTypedGroups(type);type=typedGroups[0];var groups=typedGroups[1],isCustomEvent=!isNativeTarget,selector,originalFn=fn,originalScope=scope,hasSelector,s=EVENT_SPECIAL[type];if(S.isObject(fn)){scope=fn.scope;hasSelector=('selector'in fn);selector=fn.selector;fn=fn.fn;if(selector){if(s&&s['delegateFix']){type=s['delegateFix'];}}}
if(!selector){if(s&&s['onFix']){type=s['onFix'];}}
var eventDesc=_data._data(target,undefined,isCustomEvent),events=eventDesc&&eventDesc.events,handlers,handler,len,i,j,t,special=(isNativeTarget&&EVENT_SPECIAL[type])||{};if(!events){return;}
if(!type){for(type in events){if(events.hasOwnProperty(type)){Event.__remove(isNativeTarget,target,type+groups,originalFn,originalScope);}}
return;}
var groupsRe;if(groups){groupsRe=Utils.getGroupsRe(groups);}
if((handlers=events[type])){len=handlers.length;if((fn||hasSelector||groupsRe)&&len){scope=scope||target;for(i=0,j=0,t=[];i<len;++i){handler=handlers[i];var handlerScope=handler.scope||target;if((scope!=handlerScope)||(fn&&fn!=handler.fn)||(hasSelector&&((selector&&selector!=handler.selector)||(!selector&&!handler.selector)))||(groupsRe&&!handler.groups.match(groupsRe))){t[j++]=handler;}
else{if(handler.selector&&handlers.delegateCount){handlers.delegateCount--;}
if(handler.last&&handlers.lastCount){handlers.lastCount--;}
if(special.remove){special.remove.call(target,handler);}}}
t.delegateCount=handlers.delegateCount;t.lastCount=handlers.lastCount;events[type]=t;len=t.length;}else{len=0;}
if(!len){if(isNativeTarget&&(!special['tearDown']||special['tearDown'].call(target)===false)){simpleRemove(target,type,eventDesc.handler);}
delete events[type];}}
if(S.isEmptyObject(events)){(eventDesc.handler||{}).target=null;delete eventDesc.handler;delete eventDesc.events;_data._removeData(target,isCustomEvent);}},remove:function(targets,type,fn,scope){type=S.trim(type);if(Utils.batchForType(Event.remove,targets,type,fn,scope)){return targets;}
targets=DOM.query(targets);for(var i=targets.length-1;i>=0;i--){Event.__remove(true,targets[i],type,fn,scope);}
return targets;}});},{requires:['./base','dom','./utils','./data','./special']});KISSY.add('event/special',function(){return{};});KISSY.add('event/submit',function(S,UA,Event,DOM,special){var mode=S.Env.host.document['documentMode'];if(UA['ie']&&(UA['ie']<9||(mode&&mode<9))){var getNodeName=DOM.nodeName;special['submit']={setup:function(){var el=this;if(getNodeName(el)=='form'){return false;}
Event.on(el,'click keypress',detector);},tearDown:function(){var el=this;if(getNodeName(el)=='form'){return false;}
Event.remove(el,'click keypress',detector);S.each(DOM.query('form',el),function(form){if(form.__submit__fix){form.__submit__fix=0;Event.remove(form,'submit',{fn:submitBubble,last:1});}});}};function detector(e){var t=e.target,nodeName=getNodeName(t),form=(nodeName=='input'||nodeName=='button')?t.form:null;if(form&&!form.__submit__fix){form.__submit__fix=1;Event.on(form,'submit',{fn:submitBubble,last:1});}}
function submitBubble(e){var form=this;if(form.parentNode&&!e.isPropagationStopped&&!e._ks_fired){Event.fire(form.parentNode,'submit',e);}}}},{requires:['ua','./base','dom','./special']});KISSY.add('event/target',function(S,Event,EventObject,Utils,handle,undefined){var KS_PUBLISH='__~ks_publish',trim=S.trim,splitAndRun=Utils.splitAndRun,KS_BUBBLE_TARGETS='__~ks_bubble_targets',ALL_EVENT='*';function getCustomEvent(self,type,eventData){if(eventData instanceof EventObject){eventData.currentTarget=self;return eventData;}
var customEvent=new EventObject(self,undefined,type);S.mix(customEvent,eventData);return customEvent}
function getEventPublishObj(self){self[KS_PUBLISH]=self[KS_PUBLISH]||{};return self[KS_PUBLISH];}
function getBubbleTargetsObj(self){self[KS_BUBBLE_TARGETS]=self[KS_BUBBLE_TARGETS]||{};return self[KS_BUBBLE_TARGETS];}
function canBubble(self,eventType){var publish=getEventPublishObj(self);return publish[eventType]&&publish[eventType].bubbles||publish[ALL_EVENT]&&publish[ALL_EVENT].bubbles}
function attach(method){return function(type,fn,scope){var self=this;type=trim(type);splitAndRun(type,function(t){Event['__'+method](false,self,t,fn,scope);});return self;};}
var Target={fire:function(type,eventData){var self=this,ret=undefined,r2,typedGroups,_ks_groups,customEvent;eventData=eventData||{};type=trim(type);if(type.indexOf(' ')>0){splitAndRun(type,function(t){r2=self.fire(t,eventData);if(ret!==false){ret=r2;}});return ret;}
typedGroups=Utils.getTypedGroups(type);_ks_groups=typedGroups[1];type=typedGroups[0];if(_ks_groups){_ks_groups=Utils.getGroupsRe(_ks_groups);}
S.mix(eventData,{type:type,_ks_groups:_ks_groups});customEvent=getCustomEvent(self,type,eventData);ret=handle(self,customEvent,true);if(!customEvent.isPropagationStopped&&(customEvent.target!=self||canBubble(self,type))){r2=self.bubble(type,customEvent);if(ret!==false){ret=r2;}}
return ret},publish:function(type,cfg){var self=this,publish=getEventPublishObj(self);type=trim(type);if(type){splitAndRun(type,function(t){publish[t]=cfg;});}},bubble:function(type,eventData){var self=this,ret=undefined,targets=getBubbleTargetsObj(self);S.each(targets,function(t){var r2=t.fire(type,eventData);if(ret!==false){ret=r2;}});return ret;},addTarget:function(target){var self=this,targets=getBubbleTargetsObj(self);targets[S.stamp(target)]=target;},removeTarget:function(target){var self=this,targets=getBubbleTargetsObj(self);delete targets[S.stamp(target)];},on:attach('add'),detach:attach('remove')};return Target;},{requires:['./base','./object','./utils','./handle']});KISSY.add('event/utils',function(S,DOM){var NodeType=DOM.NodeType;function isIdenticalHandler(h1,h2,el){var scope1=h1.scope||el,ret=1,scope2=h2.scope||el;if(h1.fn!==h2.fn||h1.selector!==h2.selector||h1.data!==h2.data||scope1!==scope2||h1.originalType!==h2.originalType||h1.groups!==h2.groups||h1.last!==h2.last){ret=0;}
return ret;}
function isValidTarget(target){return target&&target.nodeType!==NodeType.TEXT_NODE&&target.nodeType!==NodeType.COMMENT_NODE;}
function batchForType(fn,targets,types){if(types&&types.indexOf(' ')>0){var args=S.makeArray(arguments);S.each(types.split(/\s+/),function(type){var args2=[].concat(args);args2.splice(0,3,targets,type);fn.apply(null,args2);});return true;}
return 0;}
function splitAndRun(type,fn){S.each(type.split(/\s+/),fn);}
var doc=S.Env.host.document,simpleAdd=doc.addEventListener?function(el,type,fn,capture){if(el.addEventListener){el.addEventListener(type,fn,!!capture);}}:function(el,type,fn){if(el.attachEvent){el.attachEvent('on'+type,fn);}},simpleRemove=doc.removeEventListener?function(el,type,fn,capture){if(el.removeEventListener){el.removeEventListener(type,fn,!!capture);}}:function(el,type,fn){if(el.detachEvent){el.detachEvent('on'+type,fn);}};return{Event_Triggered:'',TRIGGERED_NONE:'trigger-none-'+S.now(),EVENT_GUID:'ksEventTargetId'+S.now(),splitAndRun:splitAndRun,batchForType:batchForType,isValidTarget:isValidTarget,isIdenticalHandler:isIdenticalHandler,simpleAdd:simpleAdd,simpleRemove:simpleRemove,getTypedGroups:function(type){if(type.indexOf('.')<0){return[type,''];}
var m=type.match(/([^.]+)?(\..+)?$/),t=m[1],ret=[t],gs=m[2];if(gs){gs=gs.split('.').sort();ret.push(gs.join('.'));}else{ret.push('');}
return ret;},getGroupsRe:function(groups){return new RegExp(groups.split('.').join('.*\\.')+'(?:\\.|$)');}};},{requires:['dom']});KISSY.add('event/valuechange',function(S,Event,DOM,special){var VALUE_CHANGE='valuechange',getNodeName=DOM.nodeName,KEY='event/valuechange',HISTORY_KEY=KEY+'/history',POLL_KEY=KEY+'/poll',interval=50;function clearPollTimer(target){if(DOM.hasData(target,POLL_KEY)){var poll=DOM.data(target,POLL_KEY);clearTimeout(poll);DOM.removeData(target,POLL_KEY);}}
function stopPoll(target){DOM.removeData(target,HISTORY_KEY);clearPollTimer(target);}
function stopPollHandler(ev){clearPollTimer(ev.target);}
function checkChange(target){var v=target.value,h=DOM.data(target,HISTORY_KEY);if(v!==h){Event.fire(target,VALUE_CHANGE,{prevVal:h,newVal:v},true);DOM.data(target,HISTORY_KEY,v);}}
function startPoll(target){if(DOM.hasData(target,POLL_KEY)){return;}
DOM.data(target,POLL_KEY,setTimeout(function(){checkChange(target);DOM.data(target,POLL_KEY,setTimeout(arguments.callee,interval));},interval));}
function startPollHandler(ev){var target=ev.target;if(ev.type=='focus'){DOM.data(target,HISTORY_KEY,target.value);}
startPoll(target);}
function webkitSpeechChangeHandler(e){checkChange(e.target);}
function monitor(target){unmonitored(target);Event.on(target,'blur',stopPollHandler);Event.on(target,'webkitspeechchange',webkitSpeechChangeHandler);Event.on(target,'mousedown keyup keydown focus',startPollHandler);}
function unmonitored(target){stopPoll(target);Event.remove(target,'blur',stopPollHandler);Event.remove(target,'webkitspeechchange',webkitSpeechChangeHandler);Event.remove(target,'mousedown keyup keydown focus',startPollHandler);}
special[VALUE_CHANGE]={setup:function(){var target=this,nodeName=getNodeName(target);if(nodeName=='input'||nodeName=='textarea'){monitor(target);}},tearDown:function(){var target=this;unmonitored(target);}};return Event;},{requires:['./base','dom','./special']});KISSY.add('json',function(S,JSON){return S.JSON={parse:function(text){if(text==null||text===''){return null;}
return JSON.parse(text);},stringify:JSON.stringify};},{requires:["json/json2"]});KISSY.add("json/json2",function(S,UA){var win=S.Env.host,JSON=win.JSON;if(!JSON||UA['ie']<9){JSON=win.JSON={};}
function f(n){return n<10?'0'+n:n;}
if(typeof Date.prototype.toJSON!=='function'){Date.prototype.toJSON=function(key){return isFinite(this.valueOf())?this.getUTCFullYear()+'-'+
f(this.getUTCMonth()+1)+'-'+
f(this.getUTCDate())+'T'+
f(this.getUTCHours())+':'+
f(this.getUTCMinutes())+':'+
f(this.getUTCSeconds())+'Z':null;};String.prototype.toJSON=Number.prototype.toJSON=Boolean.prototype.toJSON=function(key){return this.valueOf();};}
var cx=/[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,escapable=/[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,gap,indent,meta={'\b':'\\b','\t':'\\t','\n':'\\n','\f':'\\f','\r':'\\r','"':'\\"','\\':'\\\\'},rep;function quote(string){escapable['lastIndex']=0;return escapable.test(string)?'"'+string.replace(escapable,function(a){var c=meta[a];return typeof c==='string'?c:'\\u'+('0000'+a.charCodeAt(0).toString(16)).slice(-4);})+'"':'"'+string+'"';}
function str(key,holder){var i,k,v,length,mind=gap,partial,value=holder[key];if(value&&typeof value==='object'&&typeof value.toJSON==='function'){value=value.toJSON(key);}
if(typeof rep==='function'){value=rep.call(holder,key,value);}
switch(typeof value){case'string':return quote(value);case'number':return isFinite(value)?String(value):'null';case'boolean':case'null':return String(value);case'object':if(!value){return'null';}
gap+=indent;partial=[];if(Object.prototype.toString.apply(value)==='[object Array]'){length=value.length;for(i=0;i<length;i+=1){partial[i]=str(i,value)||'null';}
v=partial.length===0?'[]':gap?'[\n'+gap+
partial.join(',\n'+gap)+'\n'+
mind+']':'['+partial.join(',')+']';gap=mind;return v;}
if(rep&&typeof rep==='object'){length=rep.length;for(i=0;i<length;i+=1){k=rep[i];if(typeof k==='string'){v=str(k,value);if(v){partial.push(quote(k)+(gap?': ':':')+v);}}}}else{for(k in value){if(Object.hasOwnProperty.call(value,k)){v=str(k,value);if(v){partial.push(quote(k)+(gap?': ':':')+v);}}}}
v=partial.length===0?'{}':gap?'{\n'+gap+partial.join(',\n'+gap)+'\n'+
mind+'}':'{'+partial.join(',')+'}';gap=mind;return v;}}
if(typeof JSON.stringify!=='function'){JSON.stringify=function(value,replacer,space){var i;gap='';indent='';if(typeof space==='number'){for(i=0;i<space;i+=1){indent+=' ';}}else if(typeof space==='string'){indent=space;}
rep=replacer;if(replacer&&typeof replacer!=='function'&&(typeof replacer!=='object'||typeof replacer.length!=='number')){throw new Error('JSON.stringify');}
return str('',{'':value});};}
if(typeof JSON.parse!=='function'){JSON.parse=function(text,reviver){var j;function walk(holder,key){var k,v,value=holder[key];if(value&&typeof value==='object'){for(k in value){if(Object.hasOwnProperty.call(value,k)){v=walk(value,k);if(v!==undefined){value[k]=v;}else{delete value[k];}}}}
return reviver.call(holder,key,value);}
text=String(text);cx['lastIndex']=0;if(cx.test(text)){text=text.replace(cx,function(a){return'\\u'+
('0000'+a.charCodeAt(0).toString(16)).slice(-4);});}
if(/^[\],:{}\s]*$/.test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g,'@').replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g,']').replace(/(?:^|:|,)(?:\s*\[)+/g,''))){j=eval('('+text+')');return typeof reviver==='function'?walk({'':j},''):j;}
throw new SyntaxError('JSON.parse');};}
return JSON;},{requires:['ua']});KISSY.add('ajax',function(S,serializer,IO){var undef=undefined;function get(url,data,callback,dataType,type){if(S.isFunction(data)){dataType=callback;callback=data;data=undef;}
return IO({type:type||'get',url:url,data:data,success:callback,dataType:dataType});}
S.mix(IO,{serialize:serializer.serialize,get:get,post:function(url,data,callback,dataType){if(S.isFunction(data)){dataType=callback;callback=data;data=undef;}
return get(url,data,callback,dataType,'post');},jsonp:function(url,data,callback){if(S.isFunction(data)){callback=data;data=undef;}
return get(url,data,callback,'jsonp');},getScript:S.getScript,getJSON:function(url,data,callback){if(S.isFunction(data)){callback=data;data=undef;}
return get(url,data,callback,'json');},upload:function(url,form,data,callback,dataType){if(S.isFunction(data)){dataType=callback;callback=data;data=undef;}
return IO({url:url,type:'post',dataType:dataType,form:form,data:data,success:callback});}});S.mix(S,{'Ajax':IO,'IO':IO,ajax:IO,io:IO,jsonp:IO.jsonp});return IO;},{requires:['ajax/form-serializer','ajax/base','ajax/xhr-transport','ajax/script-transport','ajax/jsonp','ajax/form','ajax/iframe-transport','ajax/methods']});KISSY.add('ajax/base',function(S,JSON,Event,undefined){var rlocalProtocol=/^(?:about|app|app\-storage|.+\-extension|file|widget)$/,rspace=/\s+/,mirror=function(s){return s;},Promise=S.Promise,rnoContent=/^(?:GET|HEAD)$/,curLocation,Uri=S.Uri,win=S.Env.host,doc=win.document,location=win.location,simulatedLocation;try{curLocation=location.href;}catch(e){S.log('ajax/base get curLocation error: ');S.log(e);curLocation=doc.createElement('a');curLocation.href='';curLocation=curLocation.href;}
simulatedLocation=new Uri(curLocation);var isLocal=rlocalProtocol.test(simulatedLocation.getScheme()),transports={},defaultConfig={type:'GET',contentType:'application/x-www-form-urlencoded; charset=UTF-8',async:true,serializeArray:true,processData:true,accepts:{xml:'application/xml, text/xml',html:'text/html',text:'text/plain',json:'application/json, text/javascript','*':'*/*'},converters:{text:{json:JSON.parse,html:mirror,text:mirror,xml:S.parseXML}},contents:{xml:/xml/,html:/html/,json:/json/}};defaultConfig.converters.html=defaultConfig.converters.text;function setUpConfig(c){var context=c.context,ifModified=c['ifModified'];delete c.context;c=S.mix(S.clone(defaultConfig),c,{deep:true});c.context=context||c;var data,uri,type=c.type,dataType=c.dataType,query;query=c.query=new S.Uri.Query();uri=c.uri=simulatedLocation.resolve(c.url);if(!('crossDomain'in c)){c.crossDomain=!c.uri.hasSameDomainAs(simulatedLocation);}
if(c.processData&&(data=c.data)){if(S.isObject(data)){query.add(data);}else{query.reset(data);}}
type=c.type=type.toUpperCase();c.hasContent=!rnoContent.test(type);dataType=c.dataType=S.trim(dataType||'*').split(rspace);if(!('cache'in c)&&S.inArray(dataType[0],['script','jsonp'])){c.cache=false;}
var ifModifiedKeyUri;if(!c.hasContent){if(query.count()){uri.query.add(query);}
if(ifModified){ifModifiedKeyUri=uri.clone();}
if(c.cache===false){uri.query.set('_ksTS',(S.now()+'_'+S.guid()));}}
if(ifModified){c.ifModifiedKeyUri=ifModifiedKeyUri||uri.clone();}
return c;}
function fire(eventType,self){IO.fire(eventType,{ajaxConfig:self.config,xhr:self,io:self});}
function IO(c){var self=this;if(!c.url){return undefined;}
if(!(self instanceof IO)){return new IO(c);}
Promise.call(self);c=setUpConfig(c);S.mix(self,{responseData:null,config:c||{},timeoutTimer:null,responseText:null,responseXML:null,responseHeadersString:'',responseHeaders:null,requestHeaders:{},readyState:0,state:0,statusText:null,status:0,transport:null,_defer:new S.Defer(this)});var transportConstructor,transport;fire('start',self);transportConstructor=transports[c.dataType[0]]||transports['*'];transport=new transportConstructor(self);self.transport=transport;if(c.contentType){self.setRequestHeader('Content-Type',c.contentType);}
var dataType=c.dataType[0],timeoutTimer,i,timeout=c.timeout,context=c.context,headers=c.headers,accepts=c.accepts;self.setRequestHeader('Accept',dataType&&accepts[dataType]?accepts[dataType]+(dataType==='*'?'':', */*; q=0.01'):accepts['*']);for(i in headers){if(headers.hasOwnProperty(i)){self.setRequestHeader(i,headers[i]);}}
if(c.beforeSend&&(c.beforeSend.call(context,self,c)===false)){return undefined;}
function genHandler(handleStr){return function(v){if(timeoutTimer=self.timeoutTimer){clearTimeout(timeoutTimer);self.timeoutTimer=0;}
var h=c[handleStr];h&&h.apply(context,v);fire(handleStr,self);};}
self.then(genHandler('success'),genHandler('error'));self.fin(genHandler('complete'));self.readyState=1;fire('send',self);if(c.async&&timeout>0){self.timeoutTimer=setTimeout(function(){self.abort('timeout');},timeout*1000);}
try{self.state=1;transport.send();}catch(e){if(self.state<2){self._ioReady(-1,e);}else{S.error(e);}}
return undefined;}
S.mix(IO,Event.Target);S.mix(IO,{isLocal:isLocal,setupConfig:function(setting){S.mix(defaultConfig,setting,{deep:true});},setupTransport:function(name,fn){transports[name]=fn;},getTransport:function(name){return transports[name];},getConfig:function(){return defaultConfig;}});return IO;},{requires:['json','event']});KISSY.add('ajax/form-serializer',function(S,DOM){var rselectTextarea=/^(?:select|textarea)/i,rCRLF=/\r?\n/g,FormSerializer,rinput=/^(?:color|date|datetime|email|hidden|month|number|password|range|search|tel|text|time|url|week)$/i;function normalizeCRLF(v){return v.replace(rCRLF,'\r\n');}
return FormSerializer={serialize:function(forms,serializeArray){return S.param(FormSerializer.getFormData(forms),undefined,undefined,serializeArray||false);},getFormData:function(forms){var elements=[],data={};S.each(DOM.query(forms),function(el){var subs=el.elements?S.makeArray(el.elements):[el];elements.push.apply(elements,subs);});elements=S.filter(elements,function(el){return el.name&&!el.disabled&&(el.checked||rselectTextarea.test(el.nodeName)||rinput.test(el.type));});S.each(elements,function(el){var val=DOM.val(el),vs;if(S.isArray(val)){val=S.map(val,normalizeCRLF);}else{val=normalizeCRLF(val);}
vs=data[el.name];if(!vs){data[el.name]=val;return;}
if(vs&&!S.isArray(vs)){vs=data[el.name]=[vs];}
vs.push.apply(vs,S.makeArray(val));});return data;}};},{requires:['dom']});KISSY.add('ajax/form',function(S,io,DOM,FormSerializer){io.on('start',function(e){var io=e.io,form,d,enctype,dataType,formParam,tmpForm,c=io.config;if(tmpForm=c.form){form=DOM.get(tmpForm);enctype=form['encoding']||form.enctype;if(enctype.toLowerCase()!='multipart/form-data'){formParam=FormSerializer.getFormData(form);if(c.hasContent){c.query.add(formParam);}else{c.uri.query.add(formParam);if(c.ifModifiedKeyUri){c.ifModifiedKeyUri.query.add(formParam);}}}else{dataType=c.dataType;d=dataType[0];if(d=='*'){d='text';}
dataType.length=2;dataType[0]='iframe';dataType[1]=d;}}});return io;},{requires:['./base','dom','./form-serializer']});KISSY.add('ajax/iframe-transport',function(S,DOM,Event,io){var doc=S.Env.host.document,OK_CODE=200,ERROR_CODE=500,BREATH_INTERVAL=30;io.setupConfig({converters:{iframe:io.getConfig().converters.text,text:{iframe:function(text){return text;}},xml:{iframe:function(xml){return xml;}}}});function createIframe(xhr){var id=S.guid('ajax-iframe'),iframe,src=DOM.getEmptyIframeSrc();iframe=xhr.iframe=DOM.create('<iframe '+
(src?(' src="'+src+'" '):'')+' id="'+id+'"'+' name="'+id+'"'+' style="position:absolute;left:-9999px;top:-9999px;"/>');DOM.prepend(iframe,doc.body||doc.documentElement);return iframe;}
function addDataToForm(query,form,serializeArray){var ret=[],isArray,vs,i,e,keys=query.keys();S.each(keys,function(k){var data=query.get(k);isArray=S.isArray(data);vs=S.makeArray(data);for(i=0;i<vs.length;i++){e=doc.createElement('input');e.type='hidden';e.name=k+(isArray&&serializeArray?'[]':'');e.value=vs[i];DOM.append(e,form);ret.push(e);}});return ret;}
function removeFieldsFromData(fields){DOM.remove(fields);}
function IframeTransport(io){this.io=io;}
S.augment(IframeTransport,{send:function(){var self=this,io=self.io,c=io.config,fields,iframe,query=c.query,form=DOM.get(c.form);self.attrs={target:DOM.attr(form,'target')||'',action:DOM.attr(form,'action')||'',method:DOM.attr(form,'method')};self.form=form;iframe=createIframe(io);DOM.attr(form,{target:iframe.id,action:c.uri.toString(c.serializeArray),method:'post'});if(query.count()){fields=addDataToForm(query,form,c.serializeArray);}
self.fields=fields;setTimeout(function(){Event.on(iframe,'load error',self._callback,self);form.submit();},10);},_callback:function(event){var self=this,form=self.form,io=self.io,eventType=event.type,iframeDoc,iframe=io.iframe;if(!iframe){return;}
DOM.attr(form,self.attrs);removeFieldsFromData(this.fields);Event.detach(iframe);setTimeout(function(){DOM.remove(iframe);},BREATH_INTERVAL);io.iframe=null;if(eventType=='load'){iframeDoc=iframe.contentWindow.document;if(iframeDoc&&iframeDoc.body){io.responseText=S.trim(DOM.text(iframeDoc.body));if(S.startsWith(io.responseText,'<?xml')){io.responseText=undefined;}}
if(iframeDoc&&iframeDoc['XMLDocument']){io.responseXML=iframeDoc['XMLDocument'];}
else{io.responseXML=iframeDoc;}
io._ioReady(OK_CODE,'success');}else if(eventType=='error'){io._ioReady(ERROR_CODE,'error');}},abort:function(){this._callback({});}});io.setupTransport('iframe',IframeTransport);return io;},{requires:['dom','event','./base']});KISSY.add('ajax/jsonp',function(S,io){var win=S.Env.host;io.setupConfig({jsonp:'callback',jsonpCallback:function(){return S.guid('jsonp');}});io.on('start',function(e){var io=e.io,c=io.config,dataType=c.dataType;if(dataType[0]=='jsonp'){var response,cJsonpCallback=c.jsonpCallback,converters,jsonpCallback=S.isFunction(cJsonpCallback)?cJsonpCallback():cJsonpCallback,previous=win[jsonpCallback];c.uri.query.set(c.jsonp,jsonpCallback);win[jsonpCallback]=function(r){if(arguments.length>1){r=S.makeArray(arguments);}
response=[r];};io.fin(function(){win[jsonpCallback]=previous;if(previous===undefined){try{delete win[jsonpCallback];}catch(e){}}else if(response){previous(response[0]);}});converters=io.converters=io.converters||{};converters.script=converters.script||{};converters.script.json=function(){if(!response){S.error(' not call jsonpCallback: '+jsonpCallback)}
return response[0];};dataType.length=2;dataType[0]='script';dataType[1]='json';}});return io;},{requires:['./base']});KISSY.add('ajax/methods',function(S,IO,undefined){var OK_CODE=200,Promise=S.Promise,MULTIPLE_CHOICES=300,NOT_MODIFIED=304,rheaders=/^(.*?):[ \t]*([^\r\n]*)\r?$/mg;function handleResponseData(io){var text=io.responseText,xml=io.responseXML,c=io.config,cConverts=c.converters,xConverts=io.converters||{},type,contentType,responseData,contents=c.contents,dataType=c.dataType;if(text||xml){contentType=io.mimeType||io.getResponseHeader('Content-Type');while(dataType[0]=='*'){dataType.shift();}
if(!dataType.length){for(type in contents){if(contents.hasOwnProperty(type)&&contents[type].test(contentType)){if(dataType[0]!=type){dataType.unshift(type);}
break;}}}
dataType[0]=dataType[0]||'text';if(dataType[0]=='text'&&text!==undefined){responseData=text;}
else if(dataType[0]=='xml'&&xml!==undefined){responseData=xml;}else{var rawData={text:text,xml:xml};S.each(['text','xml'],function(prevType){var type=dataType[0],converter=xConverts[prevType]&&xConverts[prevType][type]||cConverts[prevType]&&cConverts[prevType][type];if(converter&&rawData[prevType]){dataType.unshift(prevType);responseData=prevType=='text'?text:xml;return false;}});}}
var prevType=dataType[0];for(var i=1;i<dataType.length;i++){type=dataType[i];var converter=xConverts[prevType]&&xConverts[prevType][type]||cConverts[prevType]&&cConverts[prevType][type];if(!converter){throw'no covert for '+prevType+' => '+type;}
responseData=converter(responseData);prevType=type;}
io.responseData=responseData;}
S.extend(IO,Promise,{setRequestHeader:function(name,value){var self=this;self.requestHeaders[name]=value;return self;},getAllResponseHeaders:function(){var self=this;return self.state===2?self.responseHeadersString:null;},getResponseHeader:function(name){var match,self=this,responseHeaders;if(self.state===2){if(!(responseHeaders=self.responseHeaders)){responseHeaders=self.responseHeaders={};while((match=rheaders.exec(self.responseHeadersString))){responseHeaders[match[1]]=match[2];}}
match=responseHeaders[name];}
return match===undefined?null:match;},overrideMimeType:function(type){var self=this;if(!self.state){self.mimeType=type;}
return self;},abort:function(statusText){var self=this;statusText=statusText||'abort';if(self.transport){self.transport.abort(statusText);}
self._ioReady(0,statusText);return self;},getNativeXhr:function(){var transport;if(transport=this.transport){return transport.nativeXhr;}},_ioReady:function(status,statusText){var self=this;if(self.state==2){return;}
self.state=2;self.readyState=4;var isSuccess;if(status>=OK_CODE&&status<MULTIPLE_CHOICES||status==NOT_MODIFIED){if(status==NOT_MODIFIED){statusText='not modified';isSuccess=true;}else{try{handleResponseData(self);statusText='success';isSuccess=true;}catch(e){S.log(e.stack||e,'error');statusText='parser error';}}}else{if(status<0){status=0;}}
self.status=status;self.statusText=statusText;var defer=self._defer;defer[isSuccess?'resolve':'reject']([self.responseData,statusText,self]);}});},{requires:['./base']});KISSY.add('ajax/script-transport',function(S,IO,_,undefined){var win=S.Env.host,doc=win.document,OK_CODE=200,ERROR_CODE=500;IO.setupConfig({accepts:{script:'text/javascript, '+'application/javascript, '+'application/ecmascript, '+'application/x-ecmascript'},contents:{script:/javascript|ecmascript/},converters:{text:{script:function(text){S.globalEval(text);return text;}}}});function ScriptTransport(io){if(!io.config.crossDomain){return new(IO.getTransport('*'))(io);}
this.io=io;return 0;}
S.augment(ScriptTransport,{send:function(){var self=this,script,io=self.io,c=io.config,head=doc['head']||doc.getElementsByTagName('head')[0]||doc.documentElement;self.head=head;script=doc.createElement('script');self.script=script;script.async='async';if(c['scriptCharset']){script.charset=c['scriptCharset'];}
script.src=c.uri.toString(c.serializeArray);script.onerror=script.onload=script.onreadystatechange=function(e){e=e||win.event;self._callback((e.type||'error').toLowerCase());};head.insertBefore(script,head.firstChild);},_callback:function(event,abort){var self=this,script=self.script,io=self.io,head=self.head;if(!script){return;}
if(abort||!script.readyState||/loaded|complete/.test(script.readyState)||event=='error'){script['onerror']=script.onload=script.onreadystatechange=null;if(head&&script.parentNode){head.removeChild(script);}
self.script=undefined;self.head=undefined;if(!abort&&event!='error'){io._ioReady(OK_CODE,'success');}
else if(event=='error'){io._ioReady(ERROR_CODE,'script error');}}},abort:function(){this._callback(0,1);}});IO.setupTransport('script',ScriptTransport);return IO;},{requires:['./base','./xhr-transport']});KISSY.add('ajax/sub-domain-transport',function(S,XhrTransportBase,Event,DOM){var rurl=/^([\w\+\.\-]+:)(?:\/\/([^\/?#:]*)(?::(\d+))?)?/,PROXY_PAGE='/sub_domain_proxy.html',doc=S.Env.host.document,iframeMap={};function SubDomainTransport(io){var self=this,c=io.config;self.io=io;c.crossDomain=false;}
S.augment(SubDomainTransport,XhrTransportBase.proto,{send:function(){var self=this,c=self.io.config,uri=c.uri,hostname=uri.getHostname(),iframe,iframeUri,iframeDesc=iframeMap[hostname];var proxy=PROXY_PAGE;if(c['xdr']&&c['xdr']['subDomain']&&c['xdr']['subDomain'].proxy){proxy=c['xdr']['subDomain'].proxy;}
if(iframeDesc&&iframeDesc.ready){self.nativeXhr=XhrTransportBase.nativeXhr(0,iframeDesc.iframe.contentWindow);if(self.nativeXhr){self.sendInternal();}else{S.error('document.domain not set correctly!');}
return;}
if(!iframeDesc){iframeDesc=iframeMap[hostname]={};iframe=iframeDesc.iframe=doc.createElement('iframe');DOM.css(iframe,{position:'absolute',left:'-9999px',top:'-9999px'});DOM.prepend(iframe,doc.body||doc.documentElement);iframeUri=new S.Uri();iframeUri.setScheme(uri.getScheme());iframeUri.setHostname(hostname);iframeUri.setPath(proxy);iframe.src=iframeUri.toString();}else{iframe=iframeDesc.iframe;}
Event.on(iframe,'load',_onLoad,self);}});function _onLoad(){var self=this,c=self.io.config,uri=c.uri,hostname=uri.getHostname(),iframeDesc=iframeMap[hostname];iframeDesc.ready=1;Event.detach(iframeDesc.iframe,'load',_onLoad,self);self.send();}
return SubDomainTransport;},{requires:['./xhr-transport-base','event','dom']});KISSY.add('ajax/xdr-flash-transport',function(S,io,DOM){var
maps={},ID='io_swf',flash,doc=S.Env.host.document,init=false;function _swf(uri,_,uid){if(init){return;}
init=true;var o='<object id="'+ID+'" type="application/x-shockwave-flash" data="'+
uri+'" width="0" height="0">'+'<param name="movie" value="'+
uri+'" />'+'<param name="FlashVars" value="yid='+
_+'&uid='+
uid+'&host=KISSY.require(\'ajax\')" />'+'<param name="allowScriptAccess" value="always" />'+'</object>',c=doc.createElement('div');DOM.prepend(c,doc.body||doc.documentElement);c.innerHTML=o;}
function XdrFlashTransport(io){S.log('use flash xdr');this.io=io;}
S.augment(XdrFlashTransport,{send:function(){var self=this,io=self.io,c=io.config,xdr=c['xdr']||{};_swf(xdr.src||(S.Config.base+'ajax/io.swf'),1,1);if(!flash){setTimeout(function(){self.send();},200);return;}
self._uid=S.guid();maps[self._uid]=self;flash.send(c.uri.toString(c.serializeArray),{id:self._uid,uid:self._uid,method:c.type,data:c.hasContent&&c.query.toString(c.serializeArray)||{}});},abort:function(){flash.abort(this._uid);},_xdrResponse:function(e,o){var self=this,ret,id=o.id,io=self.io;io.responseText=decodeURI(o.c.responseText);switch(e){case'success':ret={status:200,statusText:'success'};delete maps[id];break;case'abort':delete maps[id];break;case'timeout':case'transport error':case'failure':delete maps[id];ret={status:500,statusText:e};break;}
if(ret){io._ioReady(ret.status,ret.statusText);}}});io['applyTo']=function(_,cmd,args){var cmds=cmd.split('.').slice(1),func=io;S.each(cmds,function(c){func=func[c];});func.apply(null,args);};io['xdrReady']=function(){flash=doc.getElementById(ID);};io['xdrResponse']=function(e,o){var xhr=maps[o.uid];xhr&&xhr._xdrResponse(e,o);};return XdrFlashTransport;},{requires:['./base','dom']});KISSY.add('ajax/xhr-transport-base',function(S,io){var OK_CODE=200,win=S.Env.host,_XDomainRequest=win['XDomainRequest'],NO_CONTENT_CODE=204,NOT_FOUND_CODE=404,NO_CONTENT_CODE2=1223,XhrTransportBase={proto:{}},lastModifiedCached={},eTagCached={};io.__lastModifiedCached=lastModifiedCached;io.__eTagCached=eTagCached;function createStandardXHR(_,refWin){try{return new(refWin||win)['XMLHttpRequest']();}catch(e){}
return undefined;}
function createActiveXHR(_,refWin){try{return new(refWin||win)['ActiveXObject']('Microsoft.XMLHTTP');}catch(e){S.log('createActiveXHR error');}
return undefined;}
XhrTransportBase.nativeXhr=win['ActiveXObject']?function(crossDomain,refWin){if(crossDomain&&_XDomainRequest){return new _XDomainRequest();}
return!io.isLocal&&createStandardXHR(crossDomain,refWin)||createActiveXHR(crossDomain,refWin);}:createStandardXHR;function isInstanceOfXDomainRequest(xhr){return _XDomainRequest&&(xhr instanceof _XDomainRequest);}
S.mix(XhrTransportBase.proto,{sendInternal:function(){var self=this,io=self.io,c=io.config,nativeXhr=self.nativeXhr,type=c.type,async=c.async,username,crossDomain=c.crossDomain,mimeType=io.mimeType,requestHeaders=io.requestHeaders||{},serializeArray=c.serializeArray,url=c.uri.toString(serializeArray),xhrFields,ifModifiedKey,cacheValue,i;if(ifModifiedKey=(c.ifModifiedKeyUri&&c.ifModifiedKeyUri.toString())){if(cacheValue=lastModifiedCached[ifModifiedKey]){requestHeaders['If-Modified-Since']=cacheValue;}
if(cacheValue=eTagCached[ifModifiedKey]){requestHeaders['If-None-Match']=cacheValue;}}
if(username=c['username']){nativeXhr.open(type,url,async,username,c.password)}else{nativeXhr.open(type,url,async);}
if(xhrFields=c['xhrFields']){for(i in xhrFields){if(xhrFields.hasOwnProperty(i)){nativeXhr[i]=xhrFields[i];}}}
if(mimeType&&nativeXhr.overrideMimeType){nativeXhr.overrideMimeType(mimeType);}
if(!crossDomain&&!requestHeaders['X-Requested-With']){requestHeaders['X-Requested-With']='XMLHttpRequest';}
try{if(!crossDomain){for(i in requestHeaders){if(requestHeaders.hasOwnProperty(i)){nativeXhr.setRequestHeader(i,requestHeaders[i]);}}}}catch(e){S.log('setRequestHeader in xhr error: ');S.log(e);}
nativeXhr.send(c.hasContent&&c.query.toString(serializeArray)||null);if(!async||nativeXhr.readyState==4){self._callback();}else{if(isInstanceOfXDomainRequest(nativeXhr)){nativeXhr.onload=function(){nativeXhr.readyState=4;nativeXhr.status=200;self._callback();};nativeXhr.onerror=function(){nativeXhr.readyState=4;nativeXhr.status=500;self._callback();};}else{nativeXhr.onreadystatechange=function(){self._callback();};}}},abort:function(){this._callback(0,1);},_callback:function(event,abort){try{var self=this,nativeXhr=self.nativeXhr,io=self.io,c=io.config;if(abort||nativeXhr.readyState==4){if(isInstanceOfXDomainRequest(nativeXhr)){nativeXhr.onerror=S.noop;nativeXhr.onload=S.noop;}else{nativeXhr.onreadystatechange=S.noop;}
if(abort){if(nativeXhr.readyState!==4){nativeXhr.abort();}}else{var ifModifiedKey=c.ifModifiedKeyUri&&c.ifModifiedKeyUri.toString();var status=nativeXhr.status;if(!isInstanceOfXDomainRequest(nativeXhr)){io.responseHeadersString=nativeXhr.getAllResponseHeaders();}
if(ifModifiedKey){var lastModified=nativeXhr.getResponseHeader('Last-Modified'),eTag=nativeXhr.getResponseHeader('ETag');if(lastModified){lastModifiedCached[ifModifiedKey]=lastModified;}
if(eTag){eTagCached[eTag]=eTag;}}
var xml=nativeXhr.responseXML;if(xml&&xml.documentElement){io.responseXML=xml;}
io.responseText=nativeXhr.responseText;try{var statusText=nativeXhr.statusText;}catch(e){S.log('xhr statusText error: ');S.log(e);statusText='';}
if(!status&&io.isLocal&&!c.crossDomain){status=io.responseText?OK_CODE:NOT_FOUND_CODE;}else if(status===NO_CONTENT_CODE2){status=NO_CONTENT_CODE;}
io._ioReady(status,statusText);}}}catch(firefoxAccessException){nativeXhr.onreadystatechange=S.noop;if(!abort){io._ioReady(-1,firefoxAccessException);}}}});return XhrTransportBase;},{requires:['./base']});KISSY.add('ajax/xhr-transport',function(S,io,XhrTransportBase,SubDomainTransport,XdrFlashTransport,undefined){var win=S.Env.host,_XDomainRequest=win['XDomainRequest'],detectXhr=XhrTransportBase.nativeXhr();if(detectXhr){function getMainDomain(host){var t=host.split('.'),len=t.length,limit=len>3?3:2;if(len<limit){return t.join('.');}else{return t.reverse().slice(0,limit).reverse().join('.');}}
function XhrTransport(io){var c=io.config,crossDomain=c.crossDomain,self=this,xdrCfg=c['xdr']||{};if(crossDomain){if(getMainDomain(location.hostname)==getMainDomain(c.uri.getHostname())){return new SubDomainTransport(io);}
if(!('withCredentials'in detectXhr)&&(String(xdrCfg.use)==='flash'||!_XDomainRequest)){return new XdrFlashTransport(io);}}
self.io=io;self.nativeXhr=XhrTransportBase.nativeXhr(crossDomain);return undefined;}
S.augment(XhrTransport,XhrTransportBase.proto,{send:function(){this.sendInternal();}});io.setupTransport('*',XhrTransport);}
return io;},{requires:['./base','./xhr-transport-base','./sub-domain-transport','./xdr-flash-transport']});KISSY.add('cookie',function(S){var doc=S.Env.host.document,MILLISECONDS_OF_DAY=24*60*60*1000,encode=encodeURIComponent,decode=decodeURIComponent;function isNotEmptyString(val){return S.isString(val)&&val!=='';}
return S.Cookie={get:function(name){var ret,m;if(isNotEmptyString(name)){if((m=String(doc.cookie).match(new RegExp('(?:^| )'+name+'(?:(?:=([^;]*))|;|$)')))){ret=m[1]?decode(m[1]):'';}}
return ret;},set:function(name,val,expires,domain,path,secure){var text=String(encode(val)),date=expires;if(typeof date==='number'){date=new Date();date.setTime(date.getTime()+expires*MILLISECONDS_OF_DAY);}
if(date instanceof Date){text+='; expires='+date.toUTCString();}
if(isNotEmptyString(domain)){text+='; domain='+domain;}
if(isNotEmptyString(path)){text+='; path='+path;}
if(secure){text+='; secure';}
doc.cookie=name+'='+text;},remove:function(name,domain,path,secure){this.set(name,'',-1,domain,path,secure);}};});KISSY.add('base/attribute',function(S,undefined){Attribute.INVALID={};var INVALID=Attribute.INVALID;function normalFn(host,method){if(S.isString(method)){return host[method];}
return method;}
function __fireAttrChange(self,when,name,prevVal,newVal,subAttrName,attrName){attrName=attrName||name;return self.fire(when+S.ucfirst(name)+'Change',{attrName:attrName,subAttrName:subAttrName,prevVal:prevVal,newVal:newVal});}
function ensureNonEmpty(obj,name,create){var ret=obj[name]||{};if(create){obj[name]=ret;}
return ret;}
function getAttrs(self){return ensureNonEmpty(self,'__attrs',true);}
function getAttrVals(self){return ensureNonEmpty(self,'__attrVals',true);}
function getValueByPath(o,path){for(var i=0,len=path.length;o!=undefined&&i<len;i++){o=o[path[i]];}
return o;}
function setValueByPath(o,path,val){var len=path.length-1,s=o;if(len>=0){for(var i=0;i<len;i++){o=o[path[i]];}
if(o!=undefined){o[path[i]]=val;}else{s=undefined;}}
return s;}
function getPathNamePair(self,name){var declared=self.hasAttr(name),path;if(!declared&&name.indexOf('.')!==-1){path=name.split('.');name=path.shift();}
return{path:path,name:name};}
function getValueBySubValue(prevVal,path,value){var tmp=value;if(path){if(prevVal===undefined){tmp={};}else{tmp=S.clone(prevVal);}
setValueByPath(tmp,path,value);}
return tmp;}
function setInternal(self,name,value,opts,attrs){opts=opts||{};var ret,path,subVal,prevVal,pathNamePair=getPathNamePair(self,name),fullName=name;name=pathNamePair.name;path=pathNamePair.path;prevVal=self.get(name);if(path){subVal=getValueByPath(prevVal,path);}
if(!path&&prevVal===value){return undefined;}else if(path&&subVal===value){return undefined;}
value=getValueBySubValue(prevVal,path,value);if(!opts['silent']){if(false===__fireAttrChange(self,'before',name,prevVal,value,fullName)){return false;}}
ret=self.setInternal(name,value,opts);if(ret===false){return ret;}
if(!opts['silent']){value=getAttrVals(self)[name];__fireAttrChange(self,'after',name,prevVal,value,fullName);if(!attrs){__fireAttrChange(self,'','*',[prevVal],[value],[fullName],[name]);}else{attrs.push({prevVal:prevVal,newVal:value,attrName:name,subAttrName:fullName});}}
return self;}
function Attribute(){}
Attribute.prototype={getAttrs:function(){return getAttrs(this);},getAttrVals:function(){var self=this,o={},a,attrs=getAttrs(self);for(a in attrs){if(attrs.hasOwnProperty(a)){o[a]=self.get(a);}}
return o;},addAttr:function(name,attrConfig,override){var self=this,attrs=getAttrs(self),cfg=S.clone(attrConfig);if(!attrs[name]){attrs[name]=cfg;}else{S.mix(attrs[name],cfg,override);}
return self;},addAttrs:function(attrConfigs,initialValues){var self=this;S.each(attrConfigs,function(attrConfig,name){self.addAttr(name,attrConfig);});if(initialValues){self.set(initialValues);}
return self;},hasAttr:function(name){return name&&getAttrs(this).hasOwnProperty(name);},removeAttr:function(name){var self=this;if(self.hasAttr(name)){delete getAttrs(self)[name];delete getAttrVals(self)[name];}
return self;},set:function(name,value,opts){var self=this;if(S.isPlainObject(name)){opts=value;var all=Object(name),attrs=[],e,errors=[];for(name in all){if(all.hasOwnProperty(name)){if((e=validate(self,name,all[name],all))!==undefined){errors.push(e);}}}
if(errors.length){if(opts&&opts.error){opts.error(errors);}
return false;}
for(name in all){if(all.hasOwnProperty(name)){setInternal(self,name,all[name],opts,attrs);}}
var attrNames=[],prevVals=[],newVals=[],subAttrNames=[];S.each(attrs,function(attr){prevVals.push(attr.prevVal);newVals.push(attr.newVal);attrNames.push(attr.attrName);subAttrNames.push(attr.subAttrName);});if(attrNames.length){__fireAttrChange(self,'','*',prevVals,newVals,subAttrNames,attrNames);}
return self;}
return setInternal(self,name,value,opts);},setInternal:function(name,value,opts){var self=this,setValue,e,attrConfig=ensureNonEmpty(getAttrs(self),name,true),setter=attrConfig['setter'];e=validate(self,name,value);if(e!==undefined){if(opts.error){opts.error(e);}
return false;}
if(setter&&(setter=normalFn(self,setter))){setValue=setter.call(self,value,name);}
if(setValue===INVALID){return false;}
if(setValue!==undefined){value=setValue;}
getAttrVals(self)[name]=value;},get:function(name){var self=this,dot='.',path,declared=self.hasAttr(name),attrVals=getAttrVals(self),attrConfig,getter,ret;if(!declared&&name.indexOf(dot)!==-1){path=name.split(dot);name=path.shift();}
attrConfig=ensureNonEmpty(getAttrs(self),name);getter=attrConfig['getter'];ret=name in attrVals?attrVals[name]:getDefAttrVal(self,name);if(getter&&(getter=normalFn(self,getter))){ret=getter.call(self,ret,name);}
if(path){ret=getValueByPath(ret,path);}
return ret;},reset:function(name,opts){var self=this;if(S.isString(name)){if(self.hasAttr(name)){return self.set(name,getDefAttrVal(self,name),opts);}
else{return self;}}
opts=name;var attrs=getAttrs(self),values={};for(name in attrs){if(attrs.hasOwnProperty(name)){values[name]=getDefAttrVal(self,name);}}
self.set(values,opts);return self;}};function getDefAttrVal(self,name){var attrs=getAttrs(self),attrConfig=ensureNonEmpty(attrs,name),valFn=attrConfig.valueFn,val;if(valFn&&(valFn=normalFn(self,valFn))){val=valFn.call(self);if(val!==undefined){attrConfig.value=val;}
delete attrConfig.valueFn;attrs[name]=attrConfig;}
return attrConfig.value;}
function validate(self,name,value,all){var path,prevVal,pathNamePair;pathNamePair=getPathNamePair(self,name);name=pathNamePair.name;path=pathNamePair.path;if(path){prevVal=self.get(name);value=getValueBySubValue(prevVal,path,value);}
var attrConfig=ensureNonEmpty(getAttrs(self),name,true),e,validator=attrConfig['validator'];if(validator&&(validator=normalFn(self,validator))){e=validator.call(self,value,name,all);if(e!==undefined&&e!==true){return e;}}
return undefined;}
return Attribute;});KISSY.add('base',function(S,Attribute,Event){function Base(config){var self=this,c=self.constructor;while(c){addAttrs(self,c['ATTRS']);c=c.superclass?c.superclass.constructor:null;}
initAttrs(self,config);}
function addAttrs(host,attrs){if(attrs){for(var attr in attrs){if(attrs.hasOwnProperty(attr)){host.addAttr(attr,attrs[attr],false);}}}}
function initAttrs(host,config){if(config){for(var attr in config){if(config.hasOwnProperty(attr)){host.setInternal(attr,config[attr]);}}}}
S.augment(Base,Event.Target,Attribute);Base.Attribute=Attribute;S.Base=Base;return Base;},{requires:['base/attribute','event']});KISSY.add('anim',function(S,Anim,Easing){Anim.Easing=Easing;S.mix(S,{Anim:Anim,Easing:Anim.Easing});return Anim;},{requires:['anim/base','anim/easing','anim/color','anim/background-position']});KISSY.add('anim/background-position',function(S,DOM,Anim,Fx){function numeric(bp){bp=bp.replace(/left|top/g,'0px').replace(/right|bottom/g,'100%').replace(/([0-9\.]+)(\s|\)|$)/g,'$1px$2');var res=bp.match(/(-?[0-9\.]+)(px|%|em|pt)\s(-?[0-9\.]+)(px|%|em|pt)/);return[parseFloat(res[1]),res[2],parseFloat(res[3]),res[4]];}
function BackgroundPositionFx(){BackgroundPositionFx.superclass.constructor.apply(this,arguments);}
S.extend(BackgroundPositionFx,Fx,{load:function(){var self=this,fromUnit;BackgroundPositionFx.superclass.load.apply(self,arguments);fromUnit=self.unit=['px','px'];if(self.from){var from=numeric(self.from);self.from=[from[0],from[2]];fromUnit=[from[1],from[3]];}else{self.from=[0,0];}
if(self.to){var to=numeric(self.to);self.to=[to[0],to[2]];self.unit=[to[1],to[3]];}else{self.to=[0,0];}
if(fromUnit){if(fromUnit[0]!==self.unit[0]||fromUnit[1]!==self.unit[1]){S.log('BackgroundPosition x y unit is not same :','warn');S.log(fromUnit,'warn');S.log(self.unit,'warn');}}},interpolate:function(from,to,pos){var unit=this.unit,interpolate=BackgroundPositionFx.superclass.interpolate;return interpolate(from[0],to[0],pos)+unit[0]+' '+
interpolate(from[1],to[1],pos)+unit[1];},cur:function(){return DOM.css(this.anim.config.el,'backgroundPosition');},update:function(){var self=this,prop=self.prop,el=self.anim.config.el,from=self.from,to=self.to,val=self.interpolate(from,to,self.pos);DOM.css(el,prop,val);}});Fx.Factories['backgroundPosition']=BackgroundPositionFx;return BackgroundPositionFx;},{requires:['dom','./base','./fx']});KISSY.add('anim/base',function(S,DOM,Event,Easing,UA,AM,Fx,Q){var camelCase=DOM._camelCase,NodeType=DOM.NodeType,specialVals=['hide','show','toggle'],SHORT_HANDS={background:['backgroundPosition'],border:['borderBottomWidth','borderLeftWidth','borderRightWidth','borderTopWidth'],'borderBottom':['borderBottomWidth'],'borderLeft':['borderLeftWidth'],borderTop:['borderTopWidth'],borderRight:['borderRightWidth'],font:['fontSize','fontWeight'],margin:['marginBottom','marginLeft','marginRight','marginTop'],padding:['paddingBottom','paddingLeft','paddingRight','paddingTop']},defaultConfig={duration:1,easing:'easeNone'},NUMBER_REG=/^([+\-]=)?([\d+.\-]+)([a-z%]*)$/i;Anim.SHORT_HANDS=SHORT_HANDS;function Anim(el,props,duration,easing,complete){if(el.el){var realEl=el.el;props=el.props;delete el.el;delete el.props;return new Anim(realEl,props,el);}
var self=this,config;if(!(el=DOM.get(el))){return;}
if(!(self instanceof Anim)){return new Anim(el,props,duration,easing,complete);}
if(S.isString(props)){props=S.unparam(String(props),';',':');}else{props=S.clone(props);}
S.each(props,function(v,prop){var camelProp=S.trim(camelCase(prop));if(!camelProp){delete props[prop];}else if(prop!=camelProp){props[camelProp]=props[prop];delete props[prop];}});if(S.isPlainObject(duration)){config=S.clone(duration);}else{config={duration:parseFloat(duration)||undefined,easing:easing,complete:complete};}
config=S.merge(defaultConfig,config);config.el=el;config.props=props;self.config=config;self._duration=config.duration*1000;self['domEl']=el;self._backupProps={};self._fxs={};self.on('complete',onComplete);}
function onComplete(e){var self=this,_backupProps,complete,config=self.config;if(!S.isEmptyObject(_backupProps=self._backupProps)){DOM.css(config.el,_backupProps);}
if(complete=config.complete){complete.call(self,e);}}
function runInternal(){var self=this,config=self.config,_backupProps=self._backupProps,el=config.el,elStyle,hidden,val,prop,specialEasing=(config['specialEasing']||{}),fxs=self._fxs,props=config.props;saveRunning(self);if(self.fire('beforeStart')===false){self.stop(0);return;}
if(el.nodeType==NodeType.ELEMENT_NODE){hidden=(DOM.css(el,'display')==='none');for(prop in props){if(props.hasOwnProperty(prop)){val=props[prop];if(val=='hide'&&hidden||val=='show'&&!hidden){self.stop(1);return;}}}}
if(el.nodeType==NodeType.ELEMENT_NODE&&(props.width||props.height)){elStyle=el.style;S.mix(_backupProps,{overflow:elStyle.overflow,'overflow-x':elStyle.overflowX,'overflow-y':elStyle.overflowY});elStyle.overflow='hidden';if(DOM.css(el,'display')==='inline'&&DOM.css(el,'float')==='none'){if(UA['ie']){elStyle.zoom=1;}else{elStyle.display='inline-block';}}}
S.each(props,function(val,prop){if(!props.hasOwnProperty(prop)){return;}
var easing;if(S.isArray(val)){easing=specialEasing[prop]=val[1];props[prop]=val[0];}else{easing=specialEasing[prop]=(specialEasing[prop]||config.easing);}
if(S.isString(easing)){easing=specialEasing[prop]=Easing[easing];}
specialEasing[prop]=easing||Easing['easeNone'];});S.each(SHORT_HANDS,function(shortHands,p){var origin,val;if(val=props[p]){origin={};S.each(shortHands,function(sh){origin[sh]=DOM.css(el,sh);specialEasing[sh]=specialEasing[p];});DOM.css(el,p,val);S.each(origin,function(val,sh){props[sh]=DOM.css(el,sh);DOM.css(el,sh,val);});delete props[p];}});for(prop in props){if(!props.hasOwnProperty(prop)){continue;}
val=S.trim(props[prop]);var to,from,propCfg={prop:prop,anim:self,easing:specialEasing[prop]},fx=Fx.getFx(propCfg);if(S.inArray(val,specialVals)){_backupProps[prop]=DOM.style(el,prop);if(val=='toggle'){val=hidden?'show':'hide';}
if(val=='hide'){to=0;from=fx.cur();_backupProps.display='none';}else{from=0;to=fx.cur();DOM.css(el,prop,from);DOM.show(el);}
val=to;}else{to=val;from=fx.cur();}
val+='';var unit='',parts=val.match(NUMBER_REG);if(parts){to=parseFloat(parts[2]);unit=parts[3];if(unit&&unit!=='px'){DOM.css(el,prop,val);from=(to/fx.cur())*from;DOM.css(el,prop,from+unit);}
if(parts[1]){to=((parts[1]==='-='?-1:1)*to)+from;}}
propCfg.from=from;propCfg.to=to;propCfg.unit=unit;fx.load(propCfg);fxs[prop]=fx;}
self._startTime=S.now();AM.start(self);}
Anim.prototype={constructor:Anim,isRunning:function(){return isRunning(this);},isPaused:function(){return isPaused(this);},pause:function(){var self=this;if(self.isRunning()){self._pauseDiff=S.now()-self._startTime;AM.stop(self);removeRunning(self);savePaused(self);}
return self;},resume:function(){var self=this;if(self.isPaused()){self._startTime=S.now()-self._pauseDiff;removePaused(self);saveRunning(self);AM.start(self);}
return self;},_runInternal:runInternal,run:function(){var self=this,queueName=self.config.queue;if(queueName===false){runInternal.call(self);}else{Q.queue(self);}
return self;},_frame:function(){var self=this,prop,config=self.config,end=1,c,fx,fxs=self._fxs;for(prop in fxs){if(fxs.hasOwnProperty(prop)&&!((fx=fxs[prop]).finished)){if(config.frame){c=config.frame(fx);}
if(c==1||c==0){fx.finished=c;end&=c;}else{end&=fx.frame();if(end&&config.frame){config.frame(fx);}}}}
if((self.fire('step')===false)||end){self.stop(end);}},stop:function(finish){var self=this,config=self.config,queueName=config.queue,prop,fx,fxs=self._fxs;if(!self.isRunning()){if(queueName!==false){Q.remove(self);}
return self;}
if(finish){for(prop in fxs){if(fxs.hasOwnProperty(prop)&&!((fx=fxs[prop]).finished)){if(config.frame){config.frame(fx,1);}else{fx.frame(1);}}}
self.fire('complete');}
AM.stop(self);removeRunning(self);if(queueName!==false){Q.dequeue(self);}
return self;}};S.augment(Anim,Event.Target);var runningKey=S.guid('ks-anim-unqueued-'+S.now()+'-');function saveRunning(anim){var el=anim.config.el,allRunning=DOM.data(el,runningKey);if(!allRunning){DOM.data(el,runningKey,allRunning={});}
allRunning[S.stamp(anim)]=anim;}
function removeRunning(anim){var el=anim.config.el,allRunning=DOM.data(el,runningKey);if(allRunning){delete allRunning[S.stamp(anim)];if(S.isEmptyObject(allRunning)){DOM.removeData(el,runningKey);}}}
function isRunning(anim){var el=anim.config.el,allRunning=DOM.data(el,runningKey);if(allRunning){return!!allRunning[S.stamp(anim)];}
return 0;}
var pausedKey=S.guid('ks-anim-paused-'+S.now()+'-');function savePaused(anim){var el=anim.config.el,paused=DOM.data(el,pausedKey);if(!paused){DOM.data(el,pausedKey,paused={});}
paused[S.stamp(anim)]=anim;}
function removePaused(anim){var el=anim.config.el,paused=DOM.data(el,pausedKey);if(paused){delete paused[S.stamp(anim)];if(S.isEmptyObject(paused)){DOM.removeData(el,pausedKey);}}}
function isPaused(anim){var el=anim.config.el,paused=DOM.data(el,pausedKey);if(paused){return!!paused[S.stamp(anim)];}
return 0;}
Anim.stop=function(el,end,clearQueue,queueName){if(queueName===null||S.isString(queueName)||queueName===false){return stopQueue.apply(undefined,arguments);}
if(clearQueue){Q.removeQueues(el);}
var allRunning=DOM.data(el,runningKey),anims=S.merge(allRunning);S.each(anims,function(anim){anim.stop(end);});};S.each(['pause','resume'],function(action){Anim[action]=function(el,queueName){if(queueName===null||S.isString(queueName)||queueName===false){return pauseResumeQueue(el,queueName,action);}
pauseResumeQueue(el,undefined,action);};});function pauseResumeQueue(el,queueName,action){var allAnims=DOM.data(el,action=='resume'?pausedKey:runningKey),anims=S.merge(allAnims);S.each(anims,function(anim){if(queueName===undefined||anim.config.queue==queueName){anim[action]();}});}
function stopQueue(el,end,clearQueue,queueName){if(clearQueue&&queueName!==false){Q.removeQueue(el,queueName);}
var allRunning=DOM.data(el,runningKey),anims=S.merge(allRunning);S.each(anims,function(anim){if(anim.config.queue==queueName){anim.stop(end);}});}
Anim.isRunning=function(el){var allRunning=DOM.data(el,runningKey);return allRunning&&!S.isEmptyObject(allRunning);};Anim.isPaused=function(el){var paused=DOM.data(el,pausedKey);return paused&&!S.isEmptyObject(paused);};Anim.Q=Q;if(SHORT_HANDS){}
return Anim;},{requires:['dom','event','./easing','ua','./manager','./fx','./queue']});KISSY.add('anim/color',function(S,DOM,Anim,Fx){var HEX_BASE=16,floor=Math.floor,KEYWORDS={'black':[0,0,0],'silver':[192,192,192],'gray':[128,128,128],'white':[255,255,255],'maroon':[128,0,0],'red':[255,0,0],'purple':[128,0,128],'fuchsia':[255,0,255],'green':[0,128,0],'lime':[0,255,0],'olive':[128,128,0],'yellow':[255,255,0],'navy':[0,0,128],'blue':[0,0,255],'teal':[0,128,128],'aqua':[0,255,255]},re_RGB=/^rgb\(([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+)\)$/i,re_RGBA=/^rgba\(([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+),\s*([0-9]+)\)$/i,re_hex=/^#?([0-9A-F]{1,2})([0-9A-F]{1,2})([0-9A-F]{1,2})$/i,SHORT_HANDS=Anim.SHORT_HANDS,COLORS=['backgroundColor','borderBottomColor','borderLeftColor','borderRightColor','borderTopColor','color','outlineColor'];SHORT_HANDS['background']=SHORT_HANDS['background']||[];SHORT_HANDS['background'].push('backgroundColor');SHORT_HANDS['borderColor']=['borderBottomColor','borderLeftColor','borderRightColor','borderTopColor'];SHORT_HANDS['border'].push('borderBottomColor','borderLeftColor','borderRightColor','borderTopColor');SHORT_HANDS['borderBottom'].push('borderBottomColor');SHORT_HANDS['borderLeft'].push('borderLeftColor');SHORT_HANDS['borderRight'].push('borderRightColor');SHORT_HANDS['borderTop'].push('borderTopColor');function numericColor(val){val=(val+'');var match;if(match=val.match(re_RGB)){return[parseInt(match[1]),parseInt(match[2]),parseInt(match[3])];}
else if(match=val.match(re_RGBA)){return[parseInt(match[1]),parseInt(match[2]),parseInt(match[3]),parseInt(match[4])];}
else if(match=val.match(re_hex)){for(var i=1;i<match.length;i++){if(match[i].length<2){match[i]+=match[i];}}
return[parseInt(match[1],HEX_BASE),parseInt(match[2],HEX_BASE),parseInt(match[3],HEX_BASE)];}
if(KEYWORDS[val=val.toLowerCase()]){return KEYWORDS[val];}
S.log('only allow rgb or hex color string : '+val,'warn');return[255,255,255];}
function ColorFx(){ColorFx.superclass.constructor.apply(this,arguments);}
S.extend(ColorFx,Fx,{load:function(){var self=this;ColorFx.superclass.load.apply(self,arguments);if(self.from){self.from=numericColor(self.from);}
if(self.to){self.to=numericColor(self.to);}},interpolate:function(from,to,pos){var interpolate=ColorFx.superclass.interpolate;if(from.length==3&&to.length==3){return'rgb('+[floor(interpolate(from[0],to[0],pos)),floor(interpolate(from[1],to[1],pos)),floor(interpolate(from[2],to[2],pos))].join(', ')+')';}else if(from.length==4||to.length==4){return'rgba('+[floor(interpolate(from[0],to[0],pos)),floor(interpolate(from[1],to[1],pos)),floor(interpolate(from[2],to[2],pos)),floor(interpolate(from[3]||1,to[3]||1,pos))].join(', ')+')';}else{S.log('anim/color unknown value : '+from);}}});S.each(COLORS,function(color){Fx.Factories[color]=ColorFx;});return ColorFx;},{requires:['dom','./base','./fx']});KISSY.add('anim/easing',function(){var PI=Math.PI,pow=Math.pow,sin=Math.sin,BACK_CONST=1.70158;var Easing={swing:function(t){return(-Math.cos(t*PI)/2)+0.5;},'easeNone':function(t){return t;},'easeIn':function(t){return t*t;},easeOut:function(t){return(2-t)*t;},easeBoth:function(t){return(t*=2)<1?.5*t*t:.5*(1-(--t)*(t-2));},'easeInStrong':function(t){return t*t*t*t;},easeOutStrong:function(t){return 1-(--t)*t*t*t;},'easeBothStrong':function(t){return(t*=2)<1?.5*t*t*t*t:.5*(2-(t-=2)*t*t*t);},'elasticIn':function(t){var p=.3,s=p/4;if(t===0||t===1)return t;return-(pow(2,10*(t-=1))*sin((t-s)*(2*PI)/p));},elasticOut:function(t){var p=.3,s=p/4;if(t===0||t===1)return t;return pow(2,-10*t)*sin((t-s)*(2*PI)/p)+1;},'elasticBoth':function(t){var p=.45,s=p/4;if(t===0||(t*=2)===2)return t;if(t<1){return-.5*(pow(2,10*(t-=1))*sin((t-s)*(2*PI)/p));}
return pow(2,-10*(t-=1))*sin((t-s)*(2*PI)/p)*.5+1;},'backIn':function(t){if(t===1)t-=.001;return t*t*((BACK_CONST+1)*t-BACK_CONST);},backOut:function(t){return(t-=1)*t*((BACK_CONST+1)*t+BACK_CONST)+1;},'backBoth':function(t){var s=BACK_CONST;var m=(s*=1.525)+1;if((t*=2)<1){return.5*(t*t*(m*t-s));}
return.5*((t-=2)*t*(m*t+s)+2);},bounceIn:function(t){return 1-Easing.bounceOut(1-t);},bounceOut:function(t){var s=7.5625,r;if(t<(1/2.75)){r=s*t*t;}
else if(t<(2/2.75)){r=s*(t-=(1.5/2.75))*t+.75;}
else if(t<(2.5/2.75)){r=s*(t-=(2.25/2.75))*t+.9375;}
else{r=s*(t-=(2.625/2.75))*t+.984375;}
return r;},'bounceBoth':function(t){if(t<.5){return Easing.bounceIn(t*2)*.5;}
return Easing.bounceOut(t*2-1)*.5+.5;}};return Easing;});KISSY.add('anim/fx',function(S,DOM,undefined){function Fx(cfg){this.load(cfg);}
Fx.prototype={constructor:Fx,load:function(cfg){var self=this;S.mix(self,cfg);self.pos=0;self.unit=self.unit||'';},frame:function(end){var self=this,anim=self.anim,endFlag=0,elapsedTime;if(self.finished){return 1;}
var t=S.now(),_startTime=anim._startTime,duration=anim._duration;if(end||t>=duration+_startTime){self.pos=1;endFlag=1;}else{elapsedTime=t-_startTime;self.pos=self.easing(elapsedTime/duration);}
self.update();self.finished=self.finished||endFlag;return endFlag;},interpolate:function(from,to,pos){if(S.isNumber(from)&&S.isNumber(to)){return(from+(to-from)*pos).toFixed(3);}else{return undefined;}},update:function(){var self=this,anim=self.anim,prop=self.prop,el=anim.config.el,from=self.from,to=self.to,val=self.interpolate(from,to,self.pos);if(val===undefined){if(!self.finished){self.finished=1;DOM.css(el,prop,to);S.log(self.prop+' update directly ! : '+val+' : '+from+' : '+to);}}else{val+=self.unit;if(isAttr(el,prop)){DOM.attr(el,prop,val,1);}else{DOM.css(el,prop,val);}}},cur:function(){var self=this,prop=self.prop,el=self.anim.config.el;if(isAttr(el,prop)){return DOM.attr(el,prop,undefined,1);}
var parsed,r=DOM.css(el,prop);return isNaN(parsed=parseFloat(r))?!r||r==='auto'?0:r:parsed;}};function isAttr(el,prop){if((!el.style||el.style[prop]==null)&&DOM.attr(el,prop,undefined,1)!=null){return 1;}
return 0;}
Fx.Factories={};Fx.getFx=function(cfg){var Constructor=Fx.Factories[cfg.prop]||Fx;return new Constructor(cfg);};return Fx;},{requires:['dom']});KISSY.add('anim/manager',function(S){var stamp=S.stamp;return{interval:15,runnings:{},timer:null,start:function(anim){var self=this,kv=stamp(anim);if(self.runnings[kv]){return;}
self.runnings[kv]=anim;self.startTimer();},stop:function(anim){this.notRun(anim);},notRun:function(anim){var self=this,kv=stamp(anim);delete self.runnings[kv];if(S.isEmptyObject(self.runnings)){self.stopTimer();}},pause:function(anim){this.notRun(anim);},resume:function(anim){this.start(anim);},startTimer:function(){var self=this;if(!self.timer){self.timer=setTimeout(function(){if(!self.runFrames()){self.timer=0;self.startTimer();}else{self.stopTimer();}},self.interval);}},stopTimer:function(){var self=this,t=self.timer;if(t){clearTimeout(t);self.timer=0;}},runFrames:function(){var self=this,done=1,runnings=self.runnings;for(var r in runnings){if(runnings.hasOwnProperty(r)){done=0;runnings[r]._frame();}}
return done;}};});KISSY.add('anim/queue',function(S,DOM){var
queueCollectionKey=S.guid('ks-queue-'+S.now()+'-'),queueKey=S.guid('ks-queue-'+S.now()+'-'),processing='...';function getQueue(el,name,readOnly){name=name||queueKey;var qu,quCollection=DOM.data(el,queueCollectionKey);if(!quCollection&&!readOnly){DOM.data(el,queueCollectionKey,quCollection={});}
if(quCollection){qu=quCollection[name];if(!qu&&!readOnly){qu=quCollection[name]=[];}}
return qu;}
function removeQueue(el,name){name=name||queueKey;var quCollection=DOM.data(el,queueCollectionKey);if(quCollection){delete quCollection[name];}
if(S.isEmptyObject(quCollection)){DOM.removeData(el,queueCollectionKey);}}
var q={queueCollectionKey:queueCollectionKey,queue:function(anim){var el=anim.config.el,name=anim.config.queue,qu=getQueue(el,name);qu.push(anim);if(qu[0]!==processing){q.dequeue(anim);}
return qu;},remove:function(anim){var el=anim.config.el,name=anim.config.queue,qu=getQueue(el,name,1),index;if(qu){index=S.indexOf(anim,qu);if(index>-1){qu.splice(index,1);}}},removeQueues:function(el){DOM.removeData(el,queueCollectionKey);},removeQueue:removeQueue,dequeue:function(anim){var el=anim.config.el,name=anim.config.queue,qu=getQueue(el,name,1),nextAnim=qu&&qu.shift();if(nextAnim==processing){nextAnim=qu.shift();}
if(nextAnim){qu.unshift(processing);nextAnim._runInternal();}else{removeQueue(el,name);}}};return q;},{requires:['dom']});KISSY.add('node/anim',function(S,DOM,Anim,Node,undefined){var FX=[['height','marginTop','marginBottom','paddingTop','paddingBottom'],['width','marginLeft','marginRight','paddingLeft','paddingRight'],['opacity']];function getFxs(type,num,from){var ret=[],obj={};for(var i=from||0;i<num;i++){ret.push.apply(ret,FX[i]);}
for(i=0;i<ret.length;i++){obj[ret[i]]=type;}
return obj;}
S.augment(Node,{animate:function(var_args){var self=this,originArgs=S.makeArray(arguments);S.each(self,function(elem){var args=S.clone(originArgs),arg0=args[0];if(arg0.props){arg0.el=elem;Anim(arg0).run();}else{Anim.apply(undefined,[elem].concat(args)).run();}});return self;},stop:function(end,clearQueue,queue){var self=this;S.each(self,function(elem){Anim.stop(elem,end,clearQueue,queue);});return self;},pause:function(end,queue){var self=this;S.each(self,function(elem){Anim.pause(elem,queue);});return self;},resume:function(end,queue){var self=this;S.each(self,function(elem){Anim.resume(elem,queue);});return self;},isRunning:function(){var self=this;for(var i=0;i<self.length;i++){if(Anim.isRunning(self[i])){return true;}}
return false;},isPaused:function(){var self=this;for(var i=0;i<self.length;i++){if(Anim.isPaused(self[i])){return 1;}}
return 0;}});S.each({show:getFxs('show',3),hide:getFxs('hide',3),toggle:getFxs('toggle',3),fadeIn:getFxs('show',3,2),fadeOut:getFxs('hide',3,2),fadeToggle:getFxs('toggle',3,2),slideDown:getFxs('show',1),slideUp:getFxs('hide',1),slideToggle:getFxs('toggle',1)},function(v,k){Node.prototype[k]=function(duration,complete,easing){var self=this;if(DOM[k]&&!duration){DOM[k](self);}else{S.each(self,function(elem){Anim(elem,v,duration,easing||'easeOut',complete).run();});}
return self;};});},{requires:['dom','anim','./base']});KISSY.add('node/attach',function(S,DOM,Event,NodeList,undefined){var NLP=NodeList.prototype,makeArray=S.makeArray,DOM_INCLUDES_NORM=['nodeName','equals','contains','scrollTop','scrollLeft','height','width','innerHeight','innerWidth','outerHeight','outerWidth','addStyleSheet','appendTo','prependTo','insertBefore','before','after','insertAfter','test','hasClass','addClass','removeClass','replaceClass','toggleClass','removeAttr','hasAttr','hasProp','scrollIntoView','remove','empty','removeData','hasData','unselectable','wrap','wrapAll','replaceWith','wrapInner','unwrap'],DOM_INCLUDES_NORM_NODE_LIST=['filter','first','last','parent','closest','next','prev','clone','siblings','contents','children'],DOM_INCLUDES_NORM_IF={'attr':1,'text':0,'css':1,'style':1,'val':0,'prop':1,'offset':0,'html':0,'outerHTML':0,'data':1},EVENT_INCLUDES=['on','detach','fire','fireHandler','delegate','undelegate'];function accessNorm(fn,self,args){args.unshift(self);var ret=DOM[fn].apply(DOM,args);if(ret===undefined){return self;}
return ret;}
function accessNormList(fn,self,args){args.unshift(self);var ret=DOM[fn].apply(DOM,args);if(ret===undefined){return self;}
else if(ret===null){return null;}
return new NodeList(ret);}
function accessNormIf(fn,self,index,args){if(args[index]===undefined&&!S.isObject(args[0])){args.unshift(self);return DOM[fn].apply(DOM,args);}
return accessNorm(fn,self,args);}
S.each(DOM_INCLUDES_NORM,function(k){NLP[k]=function(){var args=makeArray(arguments);return accessNorm(k,this,args);};});S.each(DOM_INCLUDES_NORM_NODE_LIST,function(k){NLP[k]=function(){var args=makeArray(arguments);return accessNormList(k,this,args);};});S.each(DOM_INCLUDES_NORM_IF,function(index,k){NLP[k]=function(){var args=makeArray(arguments);return accessNormIf(k,this,index,args);};});S.each(EVENT_INCLUDES,function(k){NLP[k]=function(){var self=this,args=makeArray(arguments);args.unshift(self);Event[k].apply(Event,args);return self;}});},{requires:['dom','event','./base']});KISSY.add('node/base',function(S,DOM,undefined){var AP=Array.prototype,slice=AP.slice,NodeType=DOM.NodeType,push=AP.push,makeArray=S.makeArray,isNodeList=DOM._isNodeList;function NodeList(html,props,ownerDocument){var self=this,domNode;if(!(self instanceof NodeList)){return new NodeList(html,props,ownerDocument);}
if(!html){return undefined;}
else if(S.isString(html)){domNode=DOM.create(html,props,ownerDocument);if(domNode.nodeType===NodeType.DOCUMENT_FRAGMENT_NODE){push.apply(this,makeArray(domNode.childNodes));return undefined;}}
else if(S.isArray(html)||isNodeList(html)){push.apply(self,makeArray(html));return undefined;}
else{domNode=html;}
self[0]=domNode;self.length=1;return undefined;}
NodeList.prototype={length:0,item:function(index){var self=this;if(S.isNumber(index)){if(index>=self.length){return null;}else{return new NodeList(self[index]);}}else{return new NodeList(index);}},add:function(selector,context,index){if(S.isNumber(context)){index=context;context=undefined;}
var list=NodeList.all(selector,context).getDOMNodes(),ret=new NodeList(this);if(index===undefined){push.apply(ret,list);}else{var args=[index,0];args.push.apply(args,list);AP.splice.apply(ret,args);}
return ret;},slice:function(start,end){return new NodeList(slice.apply(this,arguments));},getDOMNodes:function(){return slice.call(this);},each:function(fn,context){var self=this;S.each(self,function(n,i){n=new NodeList(n);return fn.call(context||n,n,i,self);});return self;},getDOMNode:function(){return this[0];},end:function(){var self=this;return self.__parent||self;},all:function(selector){var ret,self=this;if(self.length>0){ret=NodeList.all(selector,self);}else{ret=new NodeList();}
ret.__parent=self;return ret;},one:function(selector){var self=this,all=self.all(selector),ret=all.length?all.slice(0,1):null;if(ret){ret.__parent=self;}
return ret;}};S.mix(NodeList,{all:function(selector,context){if(S.isString(selector)&&(selector=S.trim(selector))&&selector.length>=3&&S.startsWith(selector,'<')&&S.endsWith(selector,'>')){if(context){if(context['getDOMNode']){context=context[0];}
if(context.ownerDocument){context=context.ownerDocument;}}
return new NodeList(selector,undefined,context);}
return new NodeList(DOM.query(selector,context));},one:function(selector,context){var all=NodeList.all(selector,context);return all.length?all.slice(0,1):null;}});NodeList.NodeType=NodeType;return NodeList;},{requires:['dom']});KISSY.add('node',function(S,Event,Node){Node.KeyCodes=Event.KeyCodes;S.mix(S,{Node:Node,NodeList:Node,one:Node.one,all:Node.all});return Node;},{requires:['event','node/base','node/attach','node/override','node/anim']});KISSY.add('node/override',function(S,DOM,Event,NodeList){var NLP=NodeList.prototype;S.each(['append','prepend','before','after'],function(insertType){NLP[insertType]=function(html){var newNode=html,self=this;if(S.isString(newNode)){newNode=DOM.create(newNode);}
if(newNode){DOM[insertType](newNode,self);}
return self;};});S.each(['wrap','wrapAll','replaceWith','wrapInner'],function(fixType){var orig=NLP[fixType];NLP[fixType]=function(others){var self=this;if(S.isString(others)){others=NodeList.all(others,self[0].ownerDocument);}
return orig.call(self,others);};})},{requires:['dom','event','./base','./attach']});KISSY.use("ua,dom,event,node,json,ajax,anim,base,cookie");