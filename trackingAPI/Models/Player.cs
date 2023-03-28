using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace trackingAPI.Models
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime dob { get; set; }
        public int height_cm { get; set; }
        public int weight_cm { get; set; }
        public string nationality { get; set; }
        public ICollection<PlayerTeam> Teams { get; set; }        
        public int Overall { get; set; }
        public int Potential { get; set; }
        public int value_eur { get; set; }
        public int wage_eur { get; set; }
        public string player_positions { get; set; }
        public string preferred_foot { get; set; }
        public int international_reputation { get; set; }
        public int weak_foot { get; set; }
        public int skill_moves { get; set; }
        public string work_rate { get; set; }
        public string body_type { get; set; }
        public int real_face { get; set; }
        public int? release_clause_eur { get; set; }
        public string? player_tags { get; set; }
        public string? team_position { get; set; }
        public int? team_jersey_number { get; set; }
        public string? loaned_from { get; set; }
        public DateTime? joined { get; set; }
        public int? contract_valid_until { get; set; }
        public string? nation_position { get; set; }
        public int? nation_jersey_number { get; set; }
        public int? Pace { get; set; }
        public int? Shooting { get; set; }
        public int? Passing { get; set;}
        public int? Dribbling { get; set; }
        public int? Defense { get; set; }
        public int? Physical { get; set; }
        public int? gk_diving { get; set; }
        public int? gk_handling { get; set; }
        public int? gk_kicking { get; set; }
        public int? gk_reflexes { get; set; }
        public int? gk_speed { get; set; }
        public int? gk_positioning { get; set; }
        public string? player_traits { get; set; }
        public int attacking_crossing { get; set; }
        public int attacking_finishing { get; set; }
        public int attacking_heading_accuracy { get; set; }
        public int attacking_short_passing { get; set; }
        public int attacking_volleys { get; set; }
        public int skill_dribbling { get; set; }
        public int skill_curve { get; set; }
        public int skill_fk_accuracy { get; set; }
        public int skill_long_passing { get; set; }
        public int skill_ball_control { get; set; }
        public int movement_acceleration { get; set; }
        public int movement_sprint_speed { get; set; }
        public int movement_agility { get; set; }
        public int movement_reactions { get; set; }
        public int movement_balance { get; set; }
        public int power_shot_power { get; set; }
        public int power_jumping { get; set; }
        public int power_stamina { get; set; }
        public int power_strength { get; set; }
        public int power_long_shots { get; set; }
        public int mentality_aggression { get; set; }
        public int mentality_interceptions { get; set; }
        public int mentality_positioning { get; set; }
        public int mentality_vision { get; set; }
        public int mentality_penalties { get; set; }
        public int mentality_composure { get; set; }
        public int defending_marking { get; set; }
        public int defending_standing_tackle { get; set; }
        public int defending_sliding_tackle { get; set; }
        public int goalkeeping_diving { get; set; }
        public int goalkeeping_handling { get; set; }
        public int goalkeeping_kicking { get; set; }
        public int goalkeeping_positioning { get; set; }
        public int goalkeeping_reflexes { get; set; }
        public string? ls { get; set; }
        public string? st { get; set; }
        public string? rs { get; set; }
        public string? lw { get; set; }
        public string? lf { get; set; }
        public string? cf { get; set; }
        public string? rf { get; set; }
        public string? rw { get; set; }
        public string? lam { get; set; }
        public string? cam { get; set; }
        public string? ram { get; set; }
        public string? lm { get; set; }
        public string? lcm { get; set; }
        public string? cm { get; set; }
        public string? rcm { get; set; }
        public string? rm { get; set; }
        public string? lwb { get; set; }
        public string? ldm { get; set; }
        public string? cdm { get; set; }
        public string? rdm { get; set; }
        public string? rwb { get; set; }
        public string? lb { get; set; }
        public string? lcb { get; set; }
        public string? cb { get; set; }
        public string? rcb { get; set; }
        public string? rb { get; set; }
    }
}