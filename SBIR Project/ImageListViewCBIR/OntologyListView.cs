using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Update;
using VDS.RDF.Storage.Params;
using VDS.RDF.Writing;
using VDS.RDF.Writing.Formatting;
using System.IO;

namespace SBIR
{
    public class OntologyListView
    {
        private Notation3Parser parser;
        private Graph graph;
        private Notation3Writer writer;
        private string FileOnto = string.Empty;

        public OntologyListView(string FileOntoInit)
        {
            parser = new Notation3Parser();
            graph = new Graph();
            writer = new Notation3Writer();
            FileOnto = FileOntoInit;     
            parser.Load(graph, FileOnto);

        }

        public OntologyListView(Notation3Parser parser, Graph graph, Notation3Writer writer)
        {
            this.parser = parser;
            this.graph = graph;
            this.writer = writer;
        }
        public Notation3Parser Parse
        {
            get
            {
                return this.parser;
            }
            set
            {
                this.parser = value;
            }
        }

        public Graph Graph
        {
            get
            {
                return this.graph;
            }
            set
            {
                this.graph = value;
            }
        }

        public Notation3Writer Writer
        {
            get
            {
                return this.writer;
            }
            set
            {
                this.writer = value;
            }
        }

        public void CreateOntoClass(string FileClass)
        {
            graph.NamespaceMap.AddNamespace("owl", new Uri("http://www.w3.org/2002/07/owl#"));
            graph.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            graph.NamespaceMap.AddNamespace("xml", new Uri("http://www.w3.org/XML/1998/namespace"));
            graph.NamespaceMap.AddNamespace("xsd", new Uri("http://www.w3.org/2001/XMLSchema#"));
            graph.NamespaceMap.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            graph.NamespaceMap.AddNamespace("sbir", new Uri("https://sites.google.com/view/sbir-hcm/"));

            string strClasses = File.ReadAllText(FileClass);
            strClasses = strClasses.Trim();
            char[] delimiters = new char[] { '@', '\t', '\r', '\n' };
            string[] words = strClasses.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                string name = word.Trim();
                UriNode Sub = graph.CreateUriNode("sbir:" + name);
                UriNode Pre = graph.CreateUriNode("rdf:type");
                UriNode Obj = graph.CreateUriNode("owl:" + "Class");
                Triple t = new Triple(Sub, Pre, Obj);
                graph.Assert(t);
            }
            //writer.Save(graph, FileOnto);
        }

        public void CreateOntoSubClass(string superClass, string subClass)
        {
            UriNode Sub = graph.CreateUriNode("sbir:" + subClass);
            UriNode Pre = graph.CreateUriNode("rdfs:subClassOf");
            UriNode Obj = graph.CreateUriNode("sbir:" + superClass);
            Triple t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);
            //writer.Save(graph, FileOnto);
        }

        public void CreateTripleSBIR(string strSub, string strPre, String strObj)
        {
            UriNode Sub = graph.CreateUriNode("sbir:" + strSub);
            UriNode Pre = graph.CreateUriNode(strPre);
            UriNode Obj = graph.CreateUriNode("sbir:" + strObj);
            Triple t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);
            //writer.Save(graph, FileOnto);
        }

        public void CreateTripleLiteral(string strSub, string strPre, String Literal)
        {
            UriNode Sub = graph.CreateUriNode("sbir:" + strSub);
            UriNode Pre = graph.CreateUriNode(strPre);
            LiteralNode LObj = graph.CreateLiteralNode(Literal);
            Triple t = new Triple(Sub, Pre, LObj);
            graph.Assert(t);
            //writer.Save(graph, FileOnto);
        }

        public void CreateListSubClass(string superClass, List<string> LsubClass)
        {
            foreach (string subClass in LsubClass)
            {
                UriNode Sub = graph.CreateUriNode("sbir:" + subClass);
                UriNode Pre = graph.CreateUriNode("rdfs:subClassOf");
                UriNode Obj = graph.CreateUriNode("sbir:" + superClass);
                Triple t = new Triple(Sub, Pre, Obj);
                graph.Assert(t);
            }
            //writer.Save(graph, FileOnto);
        }
        public void CreateInClass(List<string> LClass, List<string> LIndividual)
        {
            int n = LClass.Count;
            for (int i = 0; i < n; i++)
            {
                UriNode Sub = graph.CreateUriNode("sbir:" + LIndividual.ElementAt(i));
                UriNode Pre = graph.CreateUriNode("rdf:type");
                UriNode Obj = graph.CreateUriNode("owl:NamedIndividual");
                Triple t = new Triple(Sub, Pre, Obj);
                graph.Assert(t);

                //Sub = graph.CreateUriNode("sbir:" + LIndividual.ElementAt(i));
                //Pre = graph.CreateUriNode("rdfs:type");
                Obj = graph.CreateUriNode("sbir:"+LClass.ElementAt(i));
                t = new Triple(Sub, Pre, Obj);
                graph.Assert(t);
            }
            //writer.Save(graph, FileOnto);
        }

        public void CreateOPClass(List<string> LClass, List<string> LObPro)
        {
            int n = LClass.Count;
            for (int i = 0; i < n; i++)
            {
                UriNode Sub = graph.CreateUriNode("sbir:" + LObPro.ElementAt(i));
                UriNode Pre = graph.CreateUriNode("rdf:type");
                UriNode Obj = graph.CreateUriNode("owl:ObjectProperty");
                Triple t = new Triple(Sub, Pre, Obj);
                graph.Assert(t);

                //Sub = graph.CreateUriNode("sbir:" + LObPro.ElementAt(i));
                Pre = graph.CreateUriNode("rdfs:domain");
                Obj = graph.CreateUriNode("sbir:" + LClass.ElementAt(i));
                t = new Triple(Sub, Pre, Obj);
                graph.Assert(t);
            }
            //writer.Save(graph, FileOnto);
        }

        public void CreateImageIndividual(string strImg, string strPre, string strObj)
        {
            UriNode Sub = graph.CreateUriNode("sbir:" + strImg);
            UriNode Pre = graph.CreateUriNode("rdf:type");
            UriNode Obj = graph.CreateUriNode("owl:NamedIndividual");
            Triple t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            // Sub = graph.CreateUriNode("sbir:" + strImg);
            Pre = graph.CreateUriNode(strPre);
            Obj = graph.CreateUriNode("sbir:" + strObj);
            t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            //writer.Save(graph, FileOnto);
        }

        public void CreateListImageIndividual(List<string> LstrImg, string strPre, string strObj)
        {
            foreach (string strImg in LstrImg)
            {
                CreateImageIndividual(strImg, strPre, strObj);
            }
        }

        public string GetNodeString(INode node)
        {
            string s = node.ToString();
            switch (node.NodeType)
            {
                case NodeType.Uri:
                    int lio = s.LastIndexOf('#');
                    if (lio == -1)
                        return s;
                    else
                        return s.Substring(lio + 1);
                case NodeType.Literal:
                    return string.Format("\"{0}\"", s);
                default:
                    return s;
            }
        }

        public List<string> QuerySPARQL(string SPARLQL)
        {
            List<string> LResult = new List<string>();
            SparqlResultSet resultSet = graph.ExecuteQuery(SPARLQL) as SparqlResultSet;
            if (resultSet.Count >0)
            {
                int len = resultSet.Count;
                for (int i = 0; i < len; i++)
                {
                    SparqlResult result = resultSet[i];
                    LResult.Add(result[0].ToString());
                }
            }
            return LResult;
        }

        //Annotation properties
        public void AnotationProperties()
        {
            List<string> LAno = new List<string>();
            LAno.Add("sbir:anoDescription");
            LAno.Add("sbir:anoFilename");
            LAno.Add("sbir:anoURI");
            foreach (string ano in LAno)
            {
                UriNode Sub = graph.CreateUriNode(ano);
                UriNode Pre = graph.CreateUriNode("rdf:type");
                UriNode Obj = graph.CreateUriNode("owl:AnnotationProperty");
                Triple t = new Triple(Sub, Pre, Obj);
                graph.Assert(t);

                //Sub = graph.CreateUriNode("sbir:anoDescription");
                Pre = graph.CreateUriNode("rdfs:range");
                Obj = graph.CreateUriNode("xsd:string");
                t = new Triple(Sub, Pre, Obj);
                graph.Assert(t);
            }

           //writer.Save(graph, FileOnto);
        }

         //Data properties
        public void Dataproperties()
        {
            List<string> LProp = new List<string>();
            LProp.Add("sbir:dprImgFilename");
            LProp.Add("sbir:dprImgListclass");
            LProp.Add("sbir:dprImgName");
            LProp.Add("sbir:dprImgPath");
            LProp.Add("sbir:dprImgURI");
            LProp.Add("sbir:dprInDescription");
            LProp.Add("sbir:dprInImage");
            LProp.Add("sbir:dprInKeywords");
            LProp.Add("sbir:dprInURI");
            foreach (string prop in LProp)
            {
                UriNode Sub = graph.CreateUriNode(prop);
                UriNode Pre = graph.CreateUriNode("rdf:type");
                UriNode Obj = graph.CreateUriNode("owl:DatatypeProperty");
                Triple t = new Triple(Sub, Pre, Obj);
                graph.Assert(t);
                //Sub = graph.CreateUriNode("sbir:imgFilename");
                Pre = graph.CreateUriNode("rdfs:range");
                Obj = graph.CreateUriNode("xsd:string");
                t = new Triple(Sub, Pre, Obj);
                graph.Assert(t);
            }
            //writer.Save(graph, FileOnto);
        }

        public void AddClassLiteral(string name, string[] Prop)
        {
            UriNode Sub = graph.CreateUriNode("sbir:" + name);
            UriNode Pre = graph.CreateUriNode("sbir:anoURI");
            LiteralNode Obj = graph.CreateLiteralNode(Prop[0]);
            Triple t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            Pre = graph.CreateUriNode("sbir:anoFilename");
            Obj = graph.CreateLiteralNode(Prop[1]);
            t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            Pre = graph.CreateUriNode("sbir:anoDescription");
            Obj = graph.CreateLiteralNode(Prop[2]);
            t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            //writer.Save(graph, FileOnto);
        }

        public void AddIndLiteral(string name, string[] Prop)
        {
            UriNode Sub = graph.CreateUriNode("sbir:" + name);
            UriNode Pre = graph.CreateUriNode("sbir:inURI");
            LiteralNode Obj = graph.CreateLiteralNode(Prop[0]);
            Triple t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            Pre = graph.CreateUriNode("sbir:inDescription");
            Obj = graph.CreateLiteralNode(Prop[1]);
            t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            Pre = graph.CreateUriNode("sbir:inImage");
            Obj = graph.CreateLiteralNode(Prop[2]);
            t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            char[] delimiters = new char[] { '@', '\t', '\r', '\n', ','};
            string[] words = Prop[3].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            Pre = graph.CreateUriNode("sbir:inKeywords");
            foreach (string word in words)
            {
                Obj = graph.CreateLiteralNode(word.Trim());
                t = new Triple(Sub, Pre, Obj);
                graph.Assert(t);
            }
            //writer.Save(graph, FileOnto);
        }

        public void AddImageLiteral(string name, string[] Prop)
        {
            UriNode Sub = graph.CreateUriNode("sbir:" + name);
            UriNode Pre = graph.CreateUriNode("sbir:imgURI");
            LiteralNode Obj = graph.CreateLiteralNode(Prop[0]);
            Triple t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            Pre = graph.CreateUriNode("sbir:imgPath");
            Obj = graph.CreateLiteralNode(Prop[1]);
            t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            Pre = graph.CreateUriNode("sbir:imgName");
            Obj = graph.CreateLiteralNode(Prop[2]);
            t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            Pre = graph.CreateUriNode("sbir:imgFilename");
            Obj = graph.CreateLiteralNode(Prop[3]);
            t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            char[] delimiters = new char[] { '@', '\t', '\r', '\n', ',' };
            string[] words = Prop[4].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            Pre = graph.CreateUriNode("sbir:imgListclass");
            foreach (string word in words)
            {
                Obj = graph.CreateLiteralNode(word.Trim());
                t = new Triple(Sub, Pre, Obj);
                graph.Assert(t);
            }
            //writer.Save(graph, FileOnto);
        }
        public void AddDataPropLiteral(string name, string[] Prop)
        {
            UriNode Sub = graph.CreateUriNode("sbir:" + name);
            UriNode Pre = graph.CreateUriNode("sbir:anoURI");
            LiteralNode Obj = graph.CreateLiteralNode(Prop[0]);
            Triple t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            Pre = graph.CreateUriNode("sbir: anoFilename");
            Obj = graph.CreateLiteralNode(Prop[1]);
            t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            Pre = graph.CreateUriNode("sbir:anoDescription");
            Obj = graph.CreateLiteralNode(Prop[2]);
            t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            //writer.Save(graph, FileOnto);
        }

        public void AddAnoPropLiteral(string name, string[] Prop)
        {
            UriNode Sub = graph.CreateUriNode("sbir:" + name);
            UriNode Pre = graph.CreateUriNode("sbir:adpURI");
            LiteralNode Obj = graph.CreateLiteralNode(Prop[0]);
            Triple t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            Pre = graph.CreateUriNode("sbir: adpFilename");
            Obj = graph.CreateLiteralNode(Prop[1]);
            t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            Pre = graph.CreateUriNode("sbir:adpDescription");
            Obj = graph.CreateLiteralNode(Prop[2]);
            t = new Triple(Sub, Pre, Obj);
            graph.Assert(t);

            //writer.Save(graph, FileOnto);
        }
    }
}
