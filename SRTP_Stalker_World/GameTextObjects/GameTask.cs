using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;

// Формат квеста в файле

//  <task id = "" prio="">
//    <title />
//    <objective key = "" >   <===-- Может быть несколько !!!
//      < text />

//      <map_location_type hint = "" />
//      <map_location_hidden />

//      <icon x="" y="" width="" height="" />

//      <object_story_id />

//      <function_complete />
//      <function_fail />
//      <function_call_complete />
//      <function_call_fail />

//      <infoportion_set_complete />
//      <infoportion_set_fail />
//      <infoportion_fail />
//      <infoportion_complete />

//      <article />
//    </ objective >
//  </ task >

namespace SRTP_Stalker_World.GameTextObjects
{
    public class GameTask : MainClass
    {
        public string Prio { get => _prio; set => _prio = value; }
        public GameText Title { get => _title; set => _title = value; }
        public Objectiv Objective { get => _objective; set => _objective = value; }
        string _prio;
        GameText _title;
        Objectiv _objective;

        public class Objectiv
        {
            string _key;
            GameText _text;
            string _map_location_type;
            GameText _map_location_type_hint;
            string _object_story_id;
            string _function_complete;
            string _function_fail;
            string _function_call_complete;
            string _function_call_fail;
            string _infoportion_set_complete;
            string _infoportion_set_fail;
            string _infoportion_fail;
            string _infoportion_complete;
            GameArticle _article;

            public string Key { get => _key; set => _key = value; }
            public GameText Text { get => _text; set => _text = value; }
            public string Map_location_type { get => _map_location_type; set => _map_location_type = value; }
            public GameText Map_location_type_hint { get => _map_location_type_hint; set => _map_location_type_hint = value; }
            public string Object_story_id { get => _object_story_id; set => _object_story_id = value; }
            public string Function_complete { get => _function_complete; set => _function_complete = value; }
            public string Function_fail { get => _function_fail; set => _function_fail = value; }
            public string Function_call_complete { get => _function_call_complete; set => _function_call_complete = value; }
            public string Function_call_fail { get => _function_call_fail; set => _function_call_fail = value; }
            public string Infoportion_set_complete { get => _infoportion_set_complete; set => _infoportion_set_complete = value; }
            public string Infoportion_set_fail { get => _infoportion_set_fail; set => _infoportion_set_fail = value; }
            public string Infoportion_fail { get => _infoportion_fail; set => _infoportion_fail = value; }
            public string Infoportion_complete { get => _infoportion_complete; set => _infoportion_complete = value; }
            public GameArticle Article { get => _article; set => _article = value; }
        }

        public GameTask(XmlNode xNode, string FileName) : base(xNode, FileName)
        {
            // дальнейшая обработка XmlNode
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void Reload()
        {
            throw new System.NotImplementedException();
        }

        public override void Save()
        {
            throw new System.NotImplementedException();
        }

        public override void Delete()
        {
            throw new System.NotImplementedException();
        }

        public override void Edit()
        {
            throw new System.NotImplementedException();
        }
    }
}