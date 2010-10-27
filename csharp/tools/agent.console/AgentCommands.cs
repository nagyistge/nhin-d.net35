﻿/* 
 Copyright (c) 2010, Direct Project
 All rights reserved.

 Authors:
    Umesh Madan     umeshma@microsoft.com
  
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
Neither the name of The Direct Project (directproject.org) nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Health.Direct;
using Health.Direct.Agent;
using Health.Direct.Agent.Config;
using Health.Direct.Common.Mime;
using Health.Direct.Common.Certificates;
using Health.Direct.Config.Tools.Command;

namespace Health.Direct.Tools.Agent
{
    public class AgentCommands
    {
        DirectAgent m_agent;
        
        public AgentCommands()
        {
        }
        
        public DirectAgent Agent
        {
            get
            {
                if (m_agent == null)
                {
                    throw new InvalidOperationException("Agent not started");
                }
                
                return m_agent;
            }
        }
        
        [Command(Name="Agent_Start")]
        public void StartAgent(string[] args)
        {
            if (m_agent != null)
            {
                return;
            }
            
            string configFile = args.GetOptionalValue(0, null);            
            if (string.IsNullOrEmpty(configFile))
            {
                m_agent = new DirectAgent("nhind.hsgincubator.com");
            }
            else
            {
                AgentSettings settings = AgentSettings.LoadFile(configFile);
                m_agent = settings.CreateAgent();
            }
        }
        
        [Command(Name = "Agent_Stop")]
        public void StopAgent(string[] args)
        {
            m_agent = null;   
        }        
        
        [Command(Name = "Agent_Process_Incoming")]
        public void ProcessIncoming(string[] args)
        {
            MessageFiles files = new MessageFiles(args);
            IncomingMessage message = this.Agent.ProcessIncoming(files.Read());
            files.Write(message.Message);
        }

        [Command(Name = "Agent_Process_Outgoing")]
        public void ProcessOutgoing(string[] args)
        {
            MessageFiles files = new MessageFiles(args);
            OutgoingMessage message = this.Agent.ProcessOutgoing(files.Read());
            files.Write(message.Message);
        }
    }
}
