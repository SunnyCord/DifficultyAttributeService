# Difficulty Attribute Service

The goal of this service is to provide difficulty attributes in a format that is identical to [osu! APIv2](https://osu.ppy.sh/docs/index.html#get-beatmap-attributes).
You might want to use this service alongside [aiosu](https://github.com/NiceAesth/aiosu) in order to speed up performance point calculations and get data for unranked beatmaps.

## Example

### POST /api/v2/beatmaps/4325843/attributes
```json
{
    "mods": [
        {
            "acronym": "DT",
            "settings": {
                "speed_change": 1.25
            }
        }
    ],
    "ruleset": "osu"
}
```

### Response:

```json
{
  "star_rating": 9.561804571063528,
  "max_combo": 828,
  "aim_difficulty": 5.051183921866809,
  "speed_difficulty": 3.9474407430881966,
  "speed_note_count": 154.23388434591348,
  "flashlight_difficulty": 3.289120553065186,
  "slider_factor": 0.7211804784626945,
  "approach_rate": 10.11999969482422,
  "overall_difficulty": 9.866666666666667
}
```

## Dependencies

- [osu!lazer](https://github.com/ppy/osu)
- Redis

## Installation

The recommended way to install is using Docker and the provided Dockerfile.

## Caching

osu! beatmaps are cached in JSON format using redis. The duration for maps with a leaderboard is 14 days. Qualified maps have 7 days and everything else is one day.
