using Newtonsoft.Json;
using PacketDesignTool.Managers.Parents;
using PacketDesignTool.Managers.Supports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PacketDesignTool.Managers
{
    public class ProjectSettingProfiler
    {
        public string Name;
        public string Description;
        public string Path;
        public Tag tag;
        public bool isUseUDP;

        [JsonIgnore]
        public Tag GetCopyTag
        {
            get
            {
                Tag copytag = new Tag(tag);

                return copytag;
            }
        }

        public ProjectSettingProfiler()
        {

        }
        public ProjectSettingProfiler(string name, string description, string path, Tag tag)
        {
            this.Name = name;
            this.Description = description;
            this.Path = path;
            this.tag = tag;
        }

        public ProjectSettingProfiler(ProjectSettingProfiler copyobj)
        {
            this.Name = copyobj.Name;
            this.Description = copyobj.Description;
            this.Path = copyobj.Path;
            this.tag = copyobj.tag;
        }

        public ProjectSettingProfiler GetCopy()
        {
            ProjectSettingProfiler copy = new ProjectSettingProfiler(this);

            return copy;
        }
    }

    public class SettingsManager : ManagerInterface
    {
        private static SettingsManager _Inst;
        public static SettingsManager Inst => _Inst ?? (_Inst = new SettingsManager());

        public Dictionary<string, ProjectSettingProfiler> SettingProfilers = new Dictionary<string, ProjectSettingProfiler>();

        public delegate void OnSettingManagerEventHandler(object sender, ProjectSettingProfiler profiler);

        public SettingsManager()
        {
            Enter("Default_Server", "기본 서버", PathProfilerSupportClass.GetInKeywordFrompath("Server"), TagManager.Inst.GetTag("Default_Server"));
            Enter("Default_Client", "기본 클라이언트", PathProfilerSupportClass.GetInKeywordFrompath("DummyClient"), TagManager.Inst.GetTag("Default_Client"));
        }

        public bool isContainPath(string Path)
        {
            foreach (var value in SettingProfilers.Values)
            {
                if (value.Path == Path) return true;

            }
            return false;
        }

        public bool isContainTagName(string TagName)
        {
            foreach (var value in SettingProfilers.Values)
            {
                if (value.GetCopyTag.Name == TagName) return true;
            }
            return false;
        }

        public bool Enter(string name, string description, string path, Tag tag, bool isUseUdp = false,  object sender = null, OnSettingManagerEventHandler OnSuccEventHandler = null, OnSettingManagerEventHandler OnFailedEventHandler = null)
        {
            if (SettingProfilers.ContainsKey(name))
            {
                if (OnFailedEventHandler != null)
                    OnFailedEventHandler.Invoke(sender, null);

                return false;
            }

            ProjectSettingProfiler profiler = new ProjectSettingProfiler(name, description, path, tag);
            profiler.isUseUDP = isUseUdp;
            SettingProfilers.Add(name, profiler);
            EventPacketManager.Inst.Enter(profiler, null);

            if (OnSuccEventHandler != null)
            {
                OnSuccEventHandler.Invoke(sender, profiler);
            }

            return true;
        }

        public void OnImport(object sender, string JsonData)
        {
            Dictionary<string, ProjectSettingProfiler> CopyPackers = JsonConvert.DeserializeObject<Dictionary<string, ProjectSettingProfiler>>(JsonData);

            SettingProfilers.Clear();

            foreach (var Packer in CopyPackers)
            {
                ProjectSettingProfiler packer = Packer.Value;
                Enter(packer.Name, packer.Description, packer.Path, packer.GetCopyTag, packer.isUseUDP);
            }
        }

        public bool Enter(ProjectSettingProfiler profiler, object sender = null, OnSettingManagerEventHandler OnSuccEventHandler = null, OnSettingManagerEventHandler OnFailedEventHandler = null)
        {
            if (SettingProfilers.ContainsKey(profiler.Name))
            {
                if (OnFailedEventHandler != null)
                    OnFailedEventHandler.Invoke(sender, null);

                return false;
            }

            SettingProfilers.Add(profiler.Name, profiler);

            if (OnSuccEventHandler != null)
            {
                OnSuccEventHandler.Invoke(sender, profiler);
            }

            return true;
        }

        public bool Remove(string Key, object sender = null, OnSettingManagerEventHandler OnSuccHandler = null, OnSettingManagerEventHandler OnFailedHandler = null)
        {
            if (SettingProfilers.TryGetValue(Key, out ProjectSettingProfiler profiler))
            {
                SettingProfilers.Remove(Key);

                //매니저 결속된 무언가 삭제
                EventPacketManager.Inst.Remove(profiler);

                if (OnSuccHandler != null)
                    OnSuccHandler.Invoke(sender, profiler);

            }
            else
            {
                if (OnFailedHandler != null)
                    OnFailedHandler.Invoke(sender, null);

                return false;
            }

            return true;
        }

        public List<ProjectSettingProfiler> GetLoadedProjectList()
        {
            List<ProjectSettingProfiler> LoadedProject = new List<ProjectSettingProfiler>();

            foreach(ProjectSettingProfiler value in SettingProfilers.Values)
            {
                ProjectSettingProfiler copy = new ProjectSettingProfiler(value);
                LoadedProject.Add(copy);
            }

            return LoadedProject;
        }

        public ProjectSettingProfiler GetProfilerSchemaMatchWithTag(Tag tag)
        {
            foreach (var profiler in SettingProfilers.Values)
            {
                if (profiler.tag.Name == tag.Name)
                    return profiler;
            }

            return null;
        }

        public void OnEnable(object sender, object EventKeyword)
        {

        }

        public void OnDisable(object sender, object EventKeyword)
        {

        }
    }
}
