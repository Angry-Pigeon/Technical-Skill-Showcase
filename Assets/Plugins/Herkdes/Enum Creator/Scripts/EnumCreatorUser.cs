using System;
using UnityEngine;

// MIT License
//
// Copyright (c) 2023 [Angry Pigeon]
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.


namespace Herkdes.EnumCreator {
    [Serializable]
    public class EnumCreatorUser {
        [field:SerializeField] public string EnumName { get; set; }
        [field:SerializeField] public string EnumPrefix { get; set; }
        [field:SerializeField] public string EnumSuffix { get; set; }
        [field:SerializeField] public string MemberPrefix { get; set; }
        [field:SerializeField] public string MemberSuffix { get; set; }
        [HideInInspector] public string[] EnumMembers { get; set; }
        [field: SerializeField] public bool hasFlags { get; set; }
        [field: SerializeField] public string Folder { get; set; }

        public EnumCreatorUser(string EnumName, string EnumPrefix, string EnumSuffix, string MemberPrefix, string MemberSuffix, string[] EnumMembers, bool hasFlags, string Folder) {
            this.EnumName = EnumName;
            this.EnumPrefix = EnumPrefix;
            this.EnumSuffix = EnumSuffix;
            this.MemberPrefix = MemberPrefix;
            this.MemberSuffix = MemberSuffix;
            this.EnumMembers = EnumMembers;
            this.hasFlags = hasFlags;
            this.Folder = Folder;
        }
        
        public EnumCreatorUser(string EnumName, string EnumPrefix, string EnumSuffix, string MemberPrefix, string MemberSuffix, string[] EnumMembers, bool hasFlags) {
            this.EnumName = EnumName;
            this.EnumPrefix = EnumPrefix;
            this.EnumSuffix = EnumSuffix;
            this.MemberPrefix = MemberPrefix;
            this.MemberSuffix = MemberSuffix;
            this.EnumMembers = EnumMembers;
            this.hasFlags = hasFlags;
        }
        

        public EnumCreatorUser(string EnumName, string EnumPrefix, string EnumSuffix, string MemberPrefix, string MemberSuffix, string[] EnumMembers) {
            this.EnumName = EnumName;
            this.EnumPrefix = EnumPrefix;
            this.EnumSuffix = EnumSuffix;
            this.MemberPrefix = MemberPrefix;
            this.MemberSuffix = MemberSuffix;
            this.EnumMembers = EnumMembers;
            this.hasFlags = false;
        }
        
        public EnumCreatorUser(string EnumName, string EnumPrefix, string EnumSuffix, string MemberPrefix, string[] EnumMembers) {
            this.EnumName = EnumName;
            this.EnumPrefix = EnumPrefix;
            this.EnumSuffix = EnumSuffix;
            this.MemberPrefix = "";
            this.MemberSuffix = MemberSuffix;
            this.EnumMembers = EnumMembers;
            this.hasFlags = false;
        }
        
        public EnumCreatorUser(string EnumName, string EnumPrefix, string EnumSuffix, string[] EnumMembers) {
            this.EnumName = EnumName;
            this.EnumPrefix = EnumPrefix;
            this.EnumSuffix = EnumSuffix;
            this.MemberPrefix = "";
            this.MemberSuffix = "";
            this.EnumMembers = EnumMembers;
            this.hasFlags = false;
        }
        
        public EnumCreatorUser(string EnumName, string EnumPrefix, string[] EnumMembers) {
            this.EnumName = EnumName;
            this.EnumPrefix = EnumPrefix;
            this.EnumSuffix = "";
            this.MemberPrefix = "";
            this.MemberSuffix = "";
            this.EnumMembers = EnumMembers;
            this.hasFlags = false;
        }
        
        public EnumCreatorUser(string EnumName, string[] EnumMembers) {
            this.EnumName = EnumName;
            this.EnumPrefix = "";
            this.EnumSuffix = "";
            this.MemberPrefix = "";
            this.MemberSuffix = "";
            this.EnumMembers = EnumMembers;
            this.hasFlags = false;
        }
        


    }
}