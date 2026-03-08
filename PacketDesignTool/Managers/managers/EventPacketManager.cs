using Newtonsoft.Json;
using PacketDesignTool.Managers.Parents;
using PacketDesignTool.Managers.Supports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketDesignTool.Managers
{
    public enum PROTOCOL_MSG_TYPE
    {
        TCP = 0, UDP = 1
    }

    public class EventPacketPacker
    {
        public Packer packer;
        public string PacketID;
        public List<ProjectSettingProfiler> LinkedProfilers = new List<ProjectSettingProfiler>();
        public PROTOCOL_MSG_TYPE MProtoType = PROTOCOL_MSG_TYPE.TCP;

        public bool isContains(ProjectSettingProfiler profiler)
        {
            foreach(var item in LinkedProfilers)
            {
                if (item.Name == profiler.Name) return true;
            }

            return false;
        }

        public bool RemoveAtName(ProjectSettingProfiler profiler)
        {
            int keyid = -1;

            for(int i = 0; i < LinkedProfilers.Count; ++i)
            {
                if (LinkedProfilers[i].Name == profiler.Name)
                    keyid = i;
            }

            if (keyid == -1) return false;

            LinkedProfilers.RemoveAt(keyid);

            return true;
        }
    }

    public class ProjectPacketPacker
    {
        public Tag tag;
        public Dictionary<string, EventPacketPacker> Packers = new Dictionary<string, EventPacketPacker>();
        public ProjectSettingProfiler profiler;

        public ProjectPacketPacker(Tag tag, ProjectSettingProfiler profiler)
        {
            this.tag = tag;
            this.profiler = profiler;
        }

        public Dictionary<string, List<ProjectSettingProfiler>> GetPackerToDictionaryLinked()
        {
            Dictionary<string, List<ProjectSettingProfiler>> profilers = new Dictionary<string, List<ProjectSettingProfiler>>();

            foreach(var packer in Packers.Values)
            {
                List<ProjectSettingProfiler> profilercopied = new List<ProjectSettingProfiler>();
                foreach(var profiler in packer.LinkedProfilers)
                {
                    profilercopied.Add(profiler);
                }

                profilers.Add(packer.PacketID, profilercopied);
            }

            return profilers;
        }

        public Dictionary<string, PROTOCOL_MSG_TYPE> GetProtocolHasPacketTypes()
        {
            Dictionary<string, PROTOCOL_MSG_TYPE> msgtypes = new Dictionary<string, PROTOCOL_MSG_TYPE>();

            foreach(var value in Packers.Values)
            {
                msgtypes.Add(value.packer.Name, value.MProtoType);
            }

            return msgtypes;
        }

        public List<Packer> GetPackerToArray()
        {
            List<Packer> packers = new List<Packer>();
            foreach(var packer in Packers.Values)
            {
                packers.Add(packer.packer);
            }

            return packers;
        }

        [JsonConstructor]
        public ProjectPacketPacker(Tag tag, Dictionary<string, EventPacketPacker> packers, ProjectSettingProfiler profiler)
        {
            this.tag = tag;
            this.profiler = profiler;

            if (packers == null)
                return;

            this.Packers = packers;
        }
    }

    public class EventPacketManager : ManagerInterface
    {
        public EventPacketManager()
        {

        }

        public delegate void OnEventPacketHandler(object sender, ProjectPacketPacker NewProjectPacker);
        public delegate void OnEventLinkHandler(object sender, ProjectSettingProfiler linkedprofiler);


        private static EventPacketManager _inst;

        public static EventPacketManager Inst => _inst ?? (_inst = new EventPacketManager());

        public Dictionary<string, ProjectPacketPacker> ProjectPackets = new Dictionary<string, ProjectPacketPacker>();

        public bool AddLinkProfiler(string Key, string PackerKey, ProjectSettingProfiler linkprofiler, object sender, OnEventLinkHandler OnSucc = null, OnEventLinkHandler OnFailed = null)
        {
            //if (Key == linkprofiler.Name)
            //{
            //    return false;
            //}

            if (ProjectPackets.TryGetValue(Key, out var ProjectPacketpacker))
            {
                if (ProjectPacketpacker.Packers.TryGetValue(PackerKey, out EventPacketPacker packer))
                {
                    if (packer.isContains(linkprofiler))
                    {
                        if (OnFailed != null)
                            OnFailed.Invoke(sender, null);

                        return false;
                    }

                    packer.LinkedProfilers.Add(linkprofiler);

                    if (OnSucc != null)
                        OnSucc.Invoke(this, linkprofiler);

                    return true;
                }
                else
                {
                    if (OnFailed != null)
                        OnFailed.Invoke(sender, null);

                    return false;
                }
            }
            else
            {
                if (OnFailed != null)
                    OnFailed.Invoke(sender, null);

                return false;
            }
        }

        public bool RemoveLinkProfiler(string Key, string PackerKey, ProjectSettingProfiler linkprofiler, object sender, OnEventLinkHandler OnSucc = null, OnEventLinkHandler OnFailed = null)
        {
            if (ProjectPackets.TryGetValue(Key, out var ProjectPacketpacker))
            {
                if (ProjectPacketpacker.Packers.TryGetValue(PackerKey, out EventPacketPacker packer))
                {
                    if (!packer.isContains(linkprofiler))
                    {
                        if (OnFailed != null)
                            OnFailed.Invoke(sender, null);

                        return false;
                    }

                    packer.RemoveAtName(linkprofiler);

                    if (OnSucc != null)
                        OnSucc.Invoke(this, linkprofiler);

                    return true;
                }
                else
                {
                    if (OnFailed != null)
                        OnFailed.Invoke(sender, null);

                    return false;
                }
            }
            else
            {
                if (OnFailed != null)
                    OnFailed.Invoke(sender, null);

                return false;
            }
        }

        public bool EnterPacker(EventPacketPacker packer, string Key, object sender = null,
            OnEventPacketHandler OnSucc = null, OnEventPacketHandler OnFailed = null)
        {
            if (ProjectPackets.TryGetValue(Key, out var ProjectPacketpacker))
            {
                if (ProjectPacketpacker.Packers.ContainsKey(packer.PacketID))
                {
                    if (OnFailed != null)
                        OnFailed.Invoke(sender, null);

                    return false;
                }

                ProjectPacketpacker.Packers.Add(packer.PacketID, packer);

                if (OnSucc != null)
                    OnSucc.Invoke(this, ProjectPacketpacker);

                return true;
            }
            else
            {
                if (OnFailed != null)
                    OnFailed.Invoke(sender, null);

                return false;
            }
        }

        public bool RemovePacker(EventPacketPacker packer, string Key, object sender = null,
            OnEventPacketHandler OnSucc = null, OnEventPacketHandler OnFailed = null)
        {
            if (ProjectPackets.TryGetValue(Key, out var ProjectPacketpacker))
            {
                if (!ProjectPacketpacker.Packers.ContainsKey(packer.PacketID))
                {
                    if (OnFailed != null)
                        OnFailed.Invoke(sender, null);

                    return false;
                }

                ProjectPacketpacker.Packers.Remove(packer.PacketID);

                if (OnSucc != null)
                    OnSucc.Invoke(this, ProjectPacketpacker);

                return true;
            }
            else
            {
                if (OnFailed != null)
                    OnFailed.Invoke(sender, null);

                return false;
            }
        }

        public bool Enter(ProjectSettingProfiler profiler, List<Packer> packers, 
            object sender = null, OnEventPacketHandler OnSuccHandler = null, OnEventPacketHandler OnFailedHandler = null, Dictionary<string, List<ProjectSettingProfiler>> copeidprofilers = null, Dictionary<string, PROTOCOL_MSG_TYPE> protocols = null)
        {
            if (ProjectPackets.ContainsKey(profiler.Name))
            {
                if (OnFailedHandler != null)
                    OnFailedHandler.Invoke(sender, null);

                return false;
            }

            Dictionary<string, EventPacketPacker> CopyPackers = new Dictionary<string, EventPacketPacker>();

            if (packers != null)
            {
                foreach (Packer packer in packers)
                {
                    EventPacketPacker NewPacker = new EventPacketPacker();
                    NewPacker.packer = packer;
                    NewPacker.PacketID = (ToolStaticLibrary.GetHashSCCode(packer.Name, profiler) + "_" + packer.Name).ToUpper();

                    if (protocols == null)
                    {
                        NewPacker.MProtoType = PROTOCOL_MSG_TYPE.TCP;
                    }
                    else
                    {
                        if(protocols.TryGetValue(packer.Name, out var t))
                        {
                            NewPacker.MProtoType = t;
                        }
                    }
                    
                    if (copeidprofilers != null)
                    {
                        if (copeidprofilers.TryGetValue(NewPacker.PacketID, out var values))
                        {
                            foreach (var kvp in values)
                            {
                                NewPacker.LinkedProfilers.Add(kvp);
                            }
                        }
                    }

                    CopyPackers.Add(NewPacker.PacketID, NewPacker);
                }
            }

            ProjectPacketPacker newpacker = new ProjectPacketPacker(profiler.tag, CopyPackers, profiler);

            ProjectPackets.Add(profiler.Name, newpacker);

            if (OnSuccHandler != null)
                OnSuccHandler.Invoke(sender, newpacker);

            return true;
        }

        public bool Remove(ProjectSettingProfiler profiler, 
            object sender = null, OnEventPacketHandler OnSuccHandler = null, OnEventPacketHandler OnFailedHandler = null)
        {
            if (ProjectPackets.TryGetValue(profiler.Name, out ProjectPacketPacker packer))
            {
                ProjectPackets.Remove(profiler.Name);

                if (OnSuccHandler != null)
                    OnSuccHandler.Invoke(sender, packer);

                return true;
            }
            else
            {
                if (OnFailedHandler != null)
                    OnFailedHandler.Invoke(sender, null);

                return false;
            }
        }

        public void OnImport(object sender, string JsonData)
        {
            ProjectPackets.Clear();

            Dictionary<string, ProjectPacketPacker> CopyPackers = JsonConvert.DeserializeObject<Dictionary<string, ProjectPacketPacker>>(JsonData);

            foreach (var Packer in CopyPackers)
            {
                ProjectPacketPacker packer = Packer.Value;
                Enter(packer.profiler, packer.GetPackerToArray(), null, null, null, packer.GetPackerToDictionaryLinked(), packer.GetProtocolHasPacketTypes());
            }
        }

        public void OnEnable(object sender, object EventKeyword)
        {

        }

        public void OnDisable(object sender, object EventKeyword)
        {

        }
    }
}
