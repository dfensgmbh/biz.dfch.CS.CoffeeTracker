﻿/**
 * Copyright 2017 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.ComponentModel.DataAnnotations;

namespace biz.dfch.CS.CoffeeTracker.Core.Model
{
    public class Coffee : BaseEntity
    {
        public Coffee()
        {
            this.Price = 0;
            this.Stock = 0;
            this.LastDelivery = DateTimeOffset.Now;
        }
        
        [Required]
        public string Brand { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public DateTimeOffset LastDelivery { get; set; }
    }
}