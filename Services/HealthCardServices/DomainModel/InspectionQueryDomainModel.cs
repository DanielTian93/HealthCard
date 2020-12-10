using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCardServices.DomainModel
{
    public class Meta
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string profile { get; set; }
    }

    public class Text
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
    }

    public class ValueCoding
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 报告完成
        /// </summary>
        public string display { get; set; }
    }



    public class IdentifierItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string system { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }




    public class Type
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CodingItem> coding { get; set; }
    }



    public class ExtensionItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ValueCoding valueCoding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string valueString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string display { get; set; }
    }


    public class Collector
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ExtensionItem> extension { get; set; }
    }

    public class CodingItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string system { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string display { get; set; }
    }


    public class BodySite
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CodingItem> coding { get; set; }
    }

    public class Collection
    {
        /// <summary>
        /// 
        /// </summary>
        public Collector collector { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string collectedDateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public BodySite bodySite { get; set; }
    }

    public class ContainedItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string resourceType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string receivedTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<IdentifierItem> identifier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Type type { get; set; }
  
        /// <summary>
        /// 
        /// </summary>
        public Collection collection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string effectiveDateTime { get; set; }
        public Code code { get; set; }
        public ValueQuantity valueQuantity { get; set; }
        public List<ReferenceRange> referenceRange { get; set; }
        public List<ComponentValueQuantity> component { get; set; }
    }
    public class ComponentValueQuantity
    {
        public ValueQuantity valueQuantity { get; set; }
    }
    public class ValueQuantity
    {
        public List<ExtensionItem> extension { get; set; }
        public string unit { get; set; }
    }
    public class ReferenceRange
    {
        public ReferenceRangeItem low { get; set; }
        public ReferenceRangeItem high { get; set; }
        public string text { get; set; }
    }

    public class ReferenceRangeItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string unit { get; set; }
    }

    public class BasedOn
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ExtensionItem> extension { get; set; }
    }



    public class Category
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CodingItem> coding { get; set; }
    }



    public class Code
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CodingItem> coding { get; set; }
    }



    public class Subject
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ExtensionItem> extension { get; set; }
    }



    public class Context
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ExtensionItem> extension { get; set; }
    }


    public class Actor
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ExtensionItem> extension { get; set; }
    }

    public class PerformerItem
    {
        /// <summary>
        /// 
        /// </summary>
        public Actor actor { get; set; }
    }

    public class SpecimenItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
    }



    public class ResultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ExtensionItem> extension { get; set; }
    }




    public class PresentedFormItem
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ExtensionItem> extension { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string data { get; set; }
        public string url { get; set; }
    }

    public class InspectionQueryResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string resourceType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Meta meta { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Text text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ExtensionItem> extension { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<IdentifierItem> identifier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ContainedItem> contained { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public BasedOn basedOn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Category category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Code code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Subject subject { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Context context { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string effectiveDateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string issued { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PerformerItem> performer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SpecimenItem> specimen { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ResultItem> result { get; set; }
        /// <summary>
        /// 甲状腺功能减退症
        /// </summary>
        public string conclusion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PresentedFormItem> presentedForm { get; set; }
    }
}
