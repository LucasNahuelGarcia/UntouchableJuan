using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    [CreateAssetMenu(menuName = "Audio/FootStepResolver", fileName = "FootStepResolver", order = 0)]
    public class FootStepResolver : ScriptableObject
    {
        private const string DefaultTerrainName = "default";
        [SerializeField] private List<FootstepTerrainMapper> _footstepTerrainMappers;
        [SerializeField] private float _distanceToGround;

        [FormerlySerializedAs("_defaultStep")] [SerializeField]
        private AudioEvent _defaultStepEvent;

        private readonly Dictionary<string, AudioEvent> _terrainFootstepDictionary =
            new Dictionary<string, AudioEvent>();

        public AudioEvent GetFootstepEvent(Transform position)
        {
            string terrainName = GetTerrainName(position);

            if (_terrainFootstepDictionary.ContainsKey(terrainName))
            {
                return _terrainFootstepDictionary[terrainName];
            }

            foreach (FootstepTerrainMapper mapper in _footstepTerrainMappers)
            {
                if (mapper.ContainedInName(terrainName))
                {
                    _terrainFootstepDictionary.Add(terrainName, mapper.FootStepEvent);
                    return mapper.FootStepEvent;
                }
            }

            _terrainFootstepDictionary.Add(terrainName, _defaultStepEvent);

            return _defaultStepEvent;
        }

        private string GetTerrainName(Transform position)
        {
            RaycastHit hit;
            Ray downRay = new Ray(position.position, Vector3.down);
            if (Physics.Raycast(downRay, out hit, _distanceToGround))
            {
                return hit.collider.name;
            }

            return DefaultTerrainName;
        }
    }
}