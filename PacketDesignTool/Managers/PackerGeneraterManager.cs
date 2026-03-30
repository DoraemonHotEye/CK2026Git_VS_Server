using Newtonsoft.Json;
using PacketDesignTool.Managers.Supports;
using PacketDesignTool.Managers.Supports.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PacketDesignTool.Managers
{
    public enum EOutPutType
    {
        CSharp = 0,
        CPP = 1,
    }

    public class PackerGeneraterManager : ManagerInterface
    {
        private static PackerGeneraterManager _Inst;
        public static PackerGeneraterManager Inst => _Inst ?? (_Inst = new PackerGeneraterManager());

        private string _Format = @"
[/{0}]
{1}
[{0}/]";

        public void Generate(string Path)
        {
            string jsonClass = JsonConvert.SerializeObject(ClassPackerManager.Inst.PackerList, Formatting.Indented);
            string json_outputClass = string.Format(_Format, ClassPackerManager.Inst.Prefix, jsonClass);

            string jsonStruct = JsonConvert.SerializeObject(StructPackerManager.Inst.PackerList, Formatting.Indented);
            string json_outputStruct = string.Format(_Format, StructPackerManager.Inst.Prefix, jsonStruct);

            string JsonSettings = JsonConvert.SerializeObject(SettingsManager.Inst.SettingProfilers, Formatting.Indented);
            string Json_outputSettings = string.Format(_Format, "Settings", JsonSettings);

            string JsonTags = JsonConvert.SerializeObject(TagManager.Inst.TagMap, Formatting.Indented);
            string Json_outputTags = string.Format(_Format, "Tags", JsonTags);

            string jsonEventPackets = JsonConvert.SerializeObject(EventPacketManager.Inst.ProjectPackets, Formatting.Indented);
            string Json_outputEventPackets = string.Format(_Format, "EventPackets", jsonEventPackets);

            string JsonOutput = json_outputClass + "\n" + "[EndLine/]" + json_outputStruct + "\n" + "[EndLine/]" + Json_outputSettings + "\n" + "[EndLine/]" + Json_outputTags + "\n" + "[EndLine/]" + Json_outputEventPackets;

            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            System.IO.File.WriteAllText(Path, JsonOutput);

        }

        public void GenerateCode(EOutPutType outputType, string Path)
        {
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }

            //TODO Generate Code
            switch (outputType)
            {
                case EOutPutType.CSharp:
                    GenerateCodeCSharp(Path);
                    break;
                case EOutPutType.CPP:
                    GenerateCodeCPP(Path);
                    break;
                default:
                    break;
            }
        }

        public void GenerateCodeCSharp(string Path)
        {
            int i = 0;

            string outputenumtype = "\n         ";
            string outputclass = "";
            string outputstruct = "";
            string outputEventpackets = "";

            foreach(var classpacker in ClassPackerManager.Inst.PackerList.Values)
            {
                string OutPutMembersFormat = "";
                string OutPutSerializeFormat = "";
                string OutPutGetSizeFormat = "";
                string OutPutDeserializeFormat = "";

                for(int j = 0; j < classpacker.MemberTypes.Count; ++j)
                {
                    OutPutMembersFormat += PacketFormatsCSharp.GetMemberTypeStringFormat(classpacker.MemberTypes[j], classpacker.MemberNames[j]) + "\n        ";
                    OutPutSerializeFormat += PacketFormatsCSharp.GetFormatPacker(classpacker.MemberTypes[j], classpacker.MemberNames[j]);
                    OutPutGetSizeFormat += PacketFormatsCSharp.GetFormatPackerSizeble(classpacker.MemberTypes[j], classpacker.MemberNames[j]) + "\n            ";
                    OutPutDeserializeFormat += PacketFormatsCSharp.GetFormatDeserialize(classpacker.MemberTypes[j], classpacker.MemberNames[j]);
                }
                outputclass += string.Format(PacketFormatsCSharp.ObjectFormat, "class", classpacker.Name, OutPutMembersFormat, OutPutGetSizeFormat, OutPutSerializeFormat, OutPutDeserializeFormat) + "\n";
            }

            foreach (var structpacker in StructPackerManager.Inst.PackerList.Values)
            {
                string OutPutMembersFormat = "";
                string OutPutSerializeFormat = "";
                string OutPutGetSizeFormat = "";
                string OutPutDeserializeFormat = "";

                for (int j = 0; j < structpacker.MemberTypes.Count; ++j)
                {
                    OutPutMembersFormat += PacketFormatsCSharp.GetMemberTypeStringFormat(structpacker.MemberTypes[j], structpacker.MemberNames[j]) + "\n        ";
                    OutPutSerializeFormat += PacketFormatsCSharp.GetFormatPacker(structpacker.MemberTypes[j], structpacker.MemberNames[j]);
                    OutPutGetSizeFormat += PacketFormatsCSharp.GetFormatPackerSizeble(structpacker.MemberTypes[j], structpacker.MemberNames[j]) + "\n            ";
                    OutPutDeserializeFormat += PacketFormatsCSharp.GetFormatDeserialize(structpacker.MemberTypes[j], structpacker.MemberNames[j]);
                }
                outputstruct += string.Format(PacketFormatsCSharp.ObjectFormat, "struct", structpacker.Name, OutPutMembersFormat, OutPutGetSizeFormat, OutPutSerializeFormat, OutPutDeserializeFormat) + "\n";
            }

            Dictionary<string, Dictionary<PROTOCOL_MSG_TYPE, string>> OutPutPacketManagers = new Dictionary<string, Dictionary<PROTOCOL_MSG_TYPE, string>>();

            foreach (var Profiler in SettingsManager.Inst.SettingProfilers.Values)
            {
                OutPutPacketManagers.Add(Profiler.Name, new Dictionary<PROTOCOL_MSG_TYPE, string>());
                OutPutPacketManagers[Profiler.Name].Add(PROTOCOL_MSG_TYPE.TCP, "");
                OutPutPacketManagers[Profiler.Name].Add(PROTOCOL_MSG_TYPE.UDP, "");
            }

            foreach (var projectpacket in EventPacketManager.Inst.ProjectPackets.Values)
            {
                foreach (var packer in projectpacket.Packers.Values)
                {
                    foreach (var linked in packer.LinkedProfilers)
                    {
                        if (OutPutPacketManagers.TryGetValue(linked.Name, out var value))
                        {
                            if (value.TryGetValue(packer.MProtoType, out string strvalue))
                            {
                                if (packer.MProtoType == PROTOCOL_MSG_TYPE.TCP)
                                {
                                    strvalue += PacketFormatsCSharp.GetFormatManagerMakeFunc(packer.packer, projectpacket.profiler, packer.PacketID.ToString());
                                }
                                else
                                {
                                    strvalue += PacketFormatsCSharp.GetFormatUManagerMakeFunc(packer.packer, projectpacket.profiler, packer.PacketID.ToString());
                                }
                                OutPutPacketManagers[linked.Name][packer.MProtoType] = strvalue;
                            }
                            OutPutPacketManagers[linked.Name] = value;
                        }
                    }

                    string OutPutMembersFormat = "";
                    string OutPutGetSizeFormat = "";
                    string OutPutSerializeFormat = "";
                    string OutPutDeserializeFormat = "";

                    string EnumFormat = string.Format(PacketFormatsCSharp.MsgIDFormat,
                        packer.PacketID, i);

                    i++;

                    for (int j = 0; j < packer.packer.MemberTypes.Count; ++j)
                    {
                        OutPutMembersFormat += PacketFormatsCSharp.GetMemberTypeStringFormat(packer.packer.MemberTypes[j], packer.packer.MemberNames[j]) + "\n        ";
                        OutPutSerializeFormat += PacketFormatsCSharp.GetFormatPacker(packer.packer.MemberTypes[j], packer.packer.MemberNames[j]);
                        OutPutGetSizeFormat += PacketFormatsCSharp.GetFormatPackerSizeble(packer.packer.MemberTypes[j], packer.packer.MemberNames[j]) + "\n            ";
                        OutPutDeserializeFormat += PacketFormatsCSharp.GetFormatDeserialize(packer.packer.MemberTypes[j], packer.packer.MemberNames[j]);
                    }

                    if (packer.MProtoType == PROTOCOL_MSG_TYPE.TCP)
                        outputEventpackets += string.Format(PacketFormatsCSharp.GetEventPacketClassFormat, ToolStaticLibrary.GetHashSCCode(packer.packer.Name, projectpacket.profiler) + "_" + packer.packer.Name, packer.PacketID, OutPutMembersFormat, OutPutDeserializeFormat, OutPutGetSizeFormat, OutPutSerializeFormat);
                    else
                        outputEventpackets += string.Format(PacketFormatsCSharp.GetEventUDPPacketClassFormat, ToolStaticLibrary.GetHashSCCode(packer.packer.Name, projectpacket.profiler) + "_" + packer.packer.Name, packer.PacketID, OutPutMembersFormat, OutPutDeserializeFormat, OutPutGetSizeFormat, OutPutSerializeFormat);
                    
                    outputenumtype += EnumFormat + "\n" + "         ";
                }
            }

            string output_code = string.Format(PacketFormatsCSharp.fileformat, outputclass, outputstruct, outputenumtype, outputEventpackets);

            foreach (var Managers in OutPutPacketManagers.Keys)
            {
                string output_PacketManager = string.Format(PacketFormatsCSharp.PacketManagerFormat, OutPutPacketManagers[Managers][PROTOCOL_MSG_TYPE.TCP]);
                string output_UPacketManager = string.Format(PacketFormatsCSharp.UPacketManagerFormat, OutPutPacketManagers[Managers][PROTOCOL_MSG_TYPE.UDP]);

                string savepath = "";

                ProjectSettingProfiler currProfiler = SettingsManager.Inst.SettingProfilers[Managers];

                if (currProfiler.Language == EOutPutType.CPP)
                    continue;

                if (currProfiler.tag.Type == TYPE.UNITY)
                {
                    savepath = currProfiler.Path + "/Assets" + "/Scripts/ServerCore/Packets";
                    System.IO.File.WriteAllText(savepath + "/Packets.cs", output_code);
                }
                else
                {
                    savepath = currProfiler.Path + "/Packets";
                }

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                System.IO.File.WriteAllText(savepath + "/PacketManager.cs", output_PacketManager);

                if (currProfiler.isUseUDP == true)
                {
                    System.IO.File.WriteAllText(savepath + "/UPacketManager.cs", output_UPacketManager);
                }
            }
            System.IO.File.WriteAllText(Path, output_code);
        }

        public void GenerateCodeCPP(string Path)
        {
            int i = 0;

            string outputenumtype = "\n    ";
            string outputclass = "";
            string outputstruct = "";
            string outputEventpackets = "";

            foreach (var classpacker in ClassPackerManager.Inst.PackerList.Values)
            {
                string OutPutMembersFormat = "";
                string OutPutSerializeFormat = "";
                string OutPutGetSizeFormat = "";
                string OutPutDeserializeFormat = "";

                for (int j = 0; j < classpacker.MemberTypes.Count; ++j)
                {
                    OutPutMembersFormat += PacketFormatsCPP.GetMemberTypeStringFormat(classpacker.MemberTypes[j], classpacker.MemberNames[j]) + "\n    ";
                    OutPutSerializeFormat += PacketFormatsCPP.GetFormatPacker(classpacker.MemberTypes[j], classpacker.MemberNames[j]) + "\n        ";
                    OutPutGetSizeFormat += PacketFormatsCPP.GetFormatPackerSizeble(classpacker.MemberTypes[j], classpacker.MemberNames[j]) + "\n        ";
                    OutPutDeserializeFormat += PacketFormatsCPP.GetDeserializeFormat(classpacker.MemberTypes[j], classpacker.MemberNames[j]) + "\n        ";
                }
                outputclass += string.Format(PacketFormatsCPP.ObjectFormat, "class", classpacker.Name, OutPutMembersFormat, OutPutGetSizeFormat, OutPutSerializeFormat, OutPutDeserializeFormat) + "\n";
            }

            foreach (var structpacker in StructPackerManager.Inst.PackerList.Values)
            {
                string OutPutMembersFormat = "";
                string OutPutSerializeFormat = "";
                string OutPutGetSizeFormat = "";
                string OutPutDeserializeFormat = "";

                for (int j = 0; j < structpacker.MemberTypes.Count; ++j)
                {
                    OutPutMembersFormat += PacketFormatsCPP.GetMemberTypeStringFormat(structpacker.MemberTypes[j], structpacker.MemberNames[j]) + "\n    ";
                    OutPutSerializeFormat += PacketFormatsCPP.GetFormatPacker(structpacker.MemberTypes[j], structpacker.MemberNames[j]) + "\n        ";
                    OutPutGetSizeFormat += PacketFormatsCPP.GetFormatPackerSizeble(structpacker.MemberTypes[j], structpacker.MemberNames[j]) + "\n        ";
                    OutPutDeserializeFormat += PacketFormatsCPP.GetDeserializeFormat(structpacker.MemberTypes[j], structpacker.MemberNames[j]) + "\n        ";
                }
                outputstruct += string.Format(PacketFormatsCPP.ObjectFormat, "struct", structpacker.Name, OutPutMembersFormat, OutPutGetSizeFormat, OutPutSerializeFormat, OutPutDeserializeFormat) + "\n";
            }

            Dictionary<string, Dictionary<PROTOCOL_MSG_TYPE, string>> OutPutPacketManagers = new Dictionary<string, Dictionary<PROTOCOL_MSG_TYPE, string>>();
            Dictionary<string, Dictionary<PROTOCOL_MSG_TYPE, string>> OutPutPacketHandlers = new Dictionary<string, Dictionary<PROTOCOL_MSG_TYPE, string>>();

            foreach (var Profiler in SettingsManager.Inst.SettingProfilers.Values)
            {
                OutPutPacketManagers.Add(Profiler.Name, new Dictionary<PROTOCOL_MSG_TYPE, string>());
                OutPutPacketManagers[Profiler.Name].Add(PROTOCOL_MSG_TYPE.TCP, "");
                OutPutPacketManagers[Profiler.Name].Add(PROTOCOL_MSG_TYPE.UDP, "");
            }

            foreach (var Profiler in SettingsManager.Inst.SettingProfilers.Values)
            {
                OutPutPacketHandlers.Add(Profiler.Name, new Dictionary<PROTOCOL_MSG_TYPE, string>());
                OutPutPacketHandlers[Profiler.Name].Add(PROTOCOL_MSG_TYPE.TCP, "");
                OutPutPacketHandlers[Profiler.Name].Add(PROTOCOL_MSG_TYPE.UDP, "");
            }

            foreach (var projectpacket in EventPacketManager.Inst.ProjectPackets.Values)
            {
                foreach (var packer in projectpacket.Packers.Values)
                {
                    foreach (var linked in packer.LinkedProfilers)
                    {
                        if (OutPutPacketManagers.TryGetValue(linked.Name, out var value))
                        {
                            if (value.TryGetValue(packer.MProtoType, out string strvalue))
                            {
                                if (packer.MProtoType == PROTOCOL_MSG_TYPE.TCP)
                                {
                                    strvalue += PacketFormatsCPP.GetFormatManagerMakeFunc(packer.packer, projectpacket.profiler, packer.PacketID.ToString());
                                }
                                else
                                {
                                    //strvalue += PacketFormatsCSharp.GetFormatUManagerMakeFunc(packer.packer, projectpacket.profiler, packer.PacketID.ToString());
                                }
                                OutPutPacketManagers[linked.Name][packer.MProtoType] = strvalue;
                            }
                            OutPutPacketManagers[linked.Name] = value;
                        }

                        if (OutPutPacketHandlers.TryGetValue(linked.Name, out var value2))
                        {
                            if (value2.TryGetValue(packer.MProtoType, out string strvalue))
                            {
                                if (packer.MProtoType == PROTOCOL_MSG_TYPE.TCP)
                                {
                                    strvalue += PacketFormatsCPP.GetFormatHandlerMakeFunc(packer.packer, projectpacket.profiler, packer.PacketID.ToString());
                                }
                                else
                                {
                                    //strvalue += PacketFormatsCSharp.GetFormatUManagerMakeFunc(packer.packer, projectpacket.profiler, packer.PacketID.ToString());
                                }
                                OutPutPacketHandlers[linked.Name][packer.MProtoType] = strvalue;
                            }
                            OutPutPacketHandlers[linked.Name] = value2;
                        }
                    }

                    string OutPutMembersFormat = "";
                    string OutPutGetSizeFormat = "";
                    string OutPutSerializeFormat = "";
                    string OutPutDeserializeFormat = "";

                    string EnumFormat = string.Format(PacketFormatsCPP.MsgIDFormat,
                        packer.PacketID, i);

                    i++;

                    for (int j = 0; j < packer.packer.MemberTypes.Count; ++j)
                    {
                        OutPutMembersFormat += PacketFormatsCPP.GetMemberTypeStringFormat(packer.packer.MemberTypes[j], packer.packer.MemberNames[j]) + "\n    ";
                        OutPutSerializeFormat += PacketFormatsCPP.GetFormatPacker(packer.packer.MemberTypes[j], packer.packer.MemberNames[j]) + "\n        ";
                        OutPutGetSizeFormat += PacketFormatsCPP.GetFormatPackerSizeble(packer.packer.MemberTypes[j], packer.packer.MemberNames[j]) + "\n        ";
                        OutPutDeserializeFormat += PacketFormatsCPP.GetDeserializeFormat(packer.packer.MemberTypes[j], packer.packer.MemberNames[j]) + "\n        ";
                    }

                    if (packer.MProtoType == PROTOCOL_MSG_TYPE.TCP)
                        outputEventpackets += string.Format(PacketFormatsCPP.GetEventPacketClassFormat, ToolStaticLibrary.GetHashSCCode(packer.packer.Name, projectpacket.profiler) + "_" + packer.packer.Name, packer.PacketID, OutPutMembersFormat, OutPutDeserializeFormat, OutPutGetSizeFormat, OutPutSerializeFormat);
                    else
                        outputEventpackets += string.Format(PacketFormatsCPP.GetEventUPacketClassFormat, ToolStaticLibrary.GetHashSCCode(packer.packer.Name, projectpacket.profiler) + "_" + packer.packer.Name, packer.PacketID, OutPutMembersFormat, OutPutDeserializeFormat, OutPutGetSizeFormat, OutPutSerializeFormat);

                    outputenumtype += EnumFormat + "\n" + "    ";
                }
            }

            string output_code = string.Format(PacketFormatsCPP.fileformat, outputclass, outputstruct, outputenumtype, outputEventpackets);

            foreach (var Managers in OutPutPacketManagers.Keys)
            {
                string output_PacketManager = string.Format(PacketFormatsCPP.PacketManagerFormat, OutPutPacketManagers[Managers][PROTOCOL_MSG_TYPE.TCP]);
                //string output_UPacketManager = string.Format(PacketFormatsCSharp.UPacketManagerFormat, OutPutPacketManagers[Managers][PROTOCOL_MSG_TYPE.UDP]);

                string savepath = "";

                ProjectSettingProfiler currProfiler = SettingsManager.Inst.SettingProfilers[Managers];

                if (currProfiler.Language == EOutPutType.CSharp)
                    continue;

                if (currProfiler.tag.Type == TYPE.UNITY)
                {
                    //savepath = currProfiler.Path + "/Assets" + "/Scripts/ServerCore/Packets";
                    //System.IO.File.WriteAllText(savepath + "/Packets.cs", output_code);
                }
                else
                {
                    savepath = currProfiler.Path + "/Manager";
                }

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                System.IO.File.WriteAllText(savepath + "/CPacketManager.cpp", output_PacketManager);

                if (currProfiler.isUseUDP == true)
                {
                    //System.IO.File.WriteAllText(savepath + "/UPacketManager.cs", output_UPacketManager);
                }
            }

            foreach (var Handlers in OutPutPacketHandlers.Keys)
            {
                string output_PacketHandler = string.Format(PacketFormatsCPP.PacketHandlerFormat, OutPutPacketHandlers[Handlers][PROTOCOL_MSG_TYPE.TCP]);
                //string output_UPacketManager = string.Format(PacketFormatsCSharp.UPacketManagerFormat, OutPutPacketManagers[Managers][PROTOCOL_MSG_TYPE.UDP]);

                string savepath = "";

                ProjectSettingProfiler currProfiler = SettingsManager.Inst.SettingProfilers[Handlers];

                if (currProfiler.Language == EOutPutType.CSharp)
                    continue;

                if (currProfiler.tag.Type == TYPE.UNITY)
                {
                    //savepath = currProfiler.Path + "/Assets" + "/Scripts/ServerCore/Packets";
                    //System.IO.File.WriteAllText(savepath + "/Packets.cs", output_code);
                }
                else
                {
                    savepath = currProfiler.Path + "/Handler";
                }

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                System.IO.File.WriteAllText(savepath + "/PacketHandler.h", output_PacketHandler);

                if (currProfiler.isUseUDP == true)
                {
                    //System.IO.File.WriteAllText(savepath + "/UPacketManager.cs", output_UPacketManager);
                }
            }

            System.IO.File.WriteAllText(Path, output_code);
        }

        public void OnEnable(object sender, object EventKeyword)
        {

        }

        public void OnDisable(object sender, object EventKeyword)
        {

        }
    }
}
