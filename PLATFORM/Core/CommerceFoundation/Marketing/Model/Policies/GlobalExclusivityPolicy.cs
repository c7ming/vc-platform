﻿using System;
using System.Collections.Generic;

namespace VirtoCommerce.Foundation.Marketing.Model.Policies
{
	/// <summary>
	/// This policy filters out all other policies if the global policy has been applied.
	/// </summary>
	public class GlobalExclusivityPolicy : IEvaluationPolicy
	{
		#region IEvaluationPolicy Members
        int _priority = 100;
        /// <summary>
        /// Gets or sets the priority the policies are executed by. The highest priority is ran first.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
		public int Priority 
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
            }
        }

        /// <summary>
        /// Filters the promotions.
        /// </summary>
        /// <param name="evaluationContext">The evaluation context.</param>
        /// <param name="records">The records, must be sorted in the order they are applied.</param>
        /// <returns></returns>
		public PromotionRecord[] FilterPromotions(IPromotionEvaluationContext evaluationContext, PromotionRecord[] records)
		{
            // applied global promotion, set to empty at the beginning
            var appliedGlobalPromotionId = String.Empty;
            var appliedRecords = new List<PromotionRecord>();

            foreach (var record in records)
            {
                if (appliedGlobalPromotionId == String.Empty)
                {
                    if (record.Reward.Promotion.ExclusionTypeId == (int)ExclusivityType.Global)
                    {
                        // set promotion id so we can filter all the rest of promotions
                        appliedGlobalPromotionId = record.Reward.Promotion.PromotionId;
                    }

                    appliedRecords.Add(record);
                }
                else // remove the rest of promotion records unless it was generated by the applied global promotion
                {
                    if (record.Reward.Promotion.PromotionId == appliedGlobalPromotionId)
                        appliedRecords.Add(record);
                }
            }

            return appliedRecords.ToArray();
		}
		#endregion


        public string Group
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}