using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PacketDesignTool.Managers
{
    public enum TYPE
    {
        NONE = 0,
        SERVER = 1,
        CLIENT = 2,
        TOOL = 3,
        UNITY = 4,
        UNREAL = 5
    }

    public class Tag
    {
        public string Name;
        public TYPE Type;
        public ushort ClientSeed;
        public string Description;

        public char GetFirstBiggerChar()
        {
            return Type.ToString().ToUpper()[0];
        }

        public Tag()
        {

        }

        public Tag(string name, TYPE TagType, ushort SeedNumber, string description)
        {
            this.Name = name;
            this.Type = TagType;
            this.ClientSeed = SeedNumber;
            Description = description;
        }

        public Tag(Tag tag)
        {
            this.Name = tag.Name;
            this.Type = tag.Type;
            this.ClientSeed = tag.ClientSeed;
            this.Description = tag.Description;
        }
    }

    public class TagManager : ManagerInterface
    {
        public delegate void OnTagEnterEventHandler(object sender, Tag NewTag);

        private static TagManager _Inst;
        public static TagManager Inst => _Inst ?? (_Inst = new TagManager());

        public Dictionary<string, Tag> TagMap = new Dictionary<string, Tag>();

        public TagManager()
        {
            EnterTag("Default_Server", TYPE.SERVER, "기본 서버");
            EnterTag("Default_Client", TYPE.CLIENT, "기본 클라이언트");
        }

        public void OnImport(object sender, string JsonData)
        {
            Dictionary<string, Tag> CopyPackers = JsonConvert.DeserializeObject<Dictionary<string, Tag>>(JsonData);

            foreach (var Packer in CopyPackers)
            {
                Tag packer = Packer.Value;
                EnterTag(packer.Name, packer.Type, packer.Description);
            }
        }

        public bool EnterTag(string name, TYPE tag, string Description, object sender = null, OnTagEnterEventHandler OnSuccEventHandler = null, OnTagEnterEventHandler OnFailedEventHandler = null)
        {
            if (TagMap.ContainsKey(name))
            {
                if (OnFailedEventHandler != null)
                    OnFailedEventHandler.Invoke(sender, null);

                return false;
            }

            //TODO 알고리즘 => Random Picker (Marcoff Chain)
            Tag _tag = new Tag(name, tag, (ushort)TagMap.Count, Description);

            TagMap.Add(name, _tag);

            if (OnSuccEventHandler != null)
                OnSuccEventHandler.Invoke(sender, _tag);

            return true;
        }

        public bool EnterTag(Tag tag, object sender = null, OnTagEnterEventHandler OnSuccEventHandler = null, OnTagEnterEventHandler OnFailedEventHandler = null)
        {
            if (TagMap.ContainsKey(tag.Name))
            {
                if (OnFailedEventHandler != null)
                    OnFailedEventHandler.Invoke(sender, tag);

                return false;
            }

            TagMap.Add(tag.Name, tag);

            if (OnSuccEventHandler != null)
                OnSuccEventHandler.Invoke(sender, tag);

            return true;
        }

        public bool RemoveTag(string name)
        {
            return TagMap.Remove(name);
        }

        public Tag GetTag(string name)
        {
            if (TagMap.TryGetValue(name, out Tag tag)) return tag;

            return null;
        }

        public Tag GetTagCopy(string name)
        {
            Tag tag = GetTag(name);

            if (tag == null)
                return null;

            Tag _copyTag = new Tag(tag);

            return _copyTag;
        }

        public List<Tag> GetTagList()
        {
            List<Tag> list = new List<Tag>();

            foreach(var tag in TagMap.Values)
            {
                Tag _Copy = new Tag(tag);
                list.Add(_Copy);
            }

            return list;
        }

        public void OnEnable(object sender, object EventKeyword)
        {

        }

        public void OnDisable(object sender, object EventKeyword)
        {

        }
    }
}
