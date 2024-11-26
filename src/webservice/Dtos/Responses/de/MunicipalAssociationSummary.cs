#region OpenPLZ API - Copyright (c) STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (c) STÜBER SYSTEMS GmbH
 *
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU Affero General Public License, version 3,
 *    as published by the Free Software Foundation.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *    GNU Affero General Public License for more details.
 *
 *    You should have received a copy of the GNU Affero General Public License
 *    along with this program. If not, see <http://www.gnu.org/licenses/>.
 *
 */
#endregion

using OpenPlzApi.DataLayer.DE;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.DE
{
    /// <summary>
    /// Reduced representation of a German municipal association (Gemeindeverband)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class MunicipalAssociationSummary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MunicipalAssociationSummary"/> class.
        /// </summary>
        /// <param name="municipalAssociation">Assigns data from <see cref="MunicipalAssociation"/></param>
        public MunicipalAssociationSummary(MunicipalAssociation municipalAssociation)
        {
            Key = municipalAssociation.Key;
            Name = municipalAssociation.Name;
            Type = municipalAssociation.Type.GetDisplayName();
        }

        /// <summary>
        /// Regional key (Regionalschlüssel)
        /// </summary>
        /// <example>07137</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Name (Name des Gemeindeverbandes)
        /// </summary>
        /// <example>Bendorf, Stadt</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Name { get; }

        /// <summary>
        /// Type (Kennzeichen des Gemeindeverbandes)
        /// </summary>
        /// <example>Verbandsfreie_Gemeinde</example>
        [Required]
        [JsonPropertyOrder(3)]
        public string Type { get; }
    }
}
