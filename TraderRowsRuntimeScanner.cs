using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace AdjustableTraderRows
{
    /// <summary>
    /// Applies the configured fixed column count to the trader-card grid.
    ///
    /// The plugin intentionally avoids changing server data. It only scans active UI
    /// objects and updates the GridLayoutGroup that looks like the trader selection list.
    ///
    /// This approach is more tolerant to EFT/SPT UI method-name changes than patching a
    /// single obfuscated method signature.
    /// </summary>
    internal sealed class TraderRowsRuntimeScanner : MonoBehaviour
    {
        private static readonly string[] KnownTraderNames =
        {
            "prapor",
            "therapist",
            "la toubib",
            "fence",
            "skier",
            "peacekeeper",
            "mechanic",
            "le mécano",
            "ragman",
            "jaeger",
            "ref"
        };

        private float _nextScanTime;
        private readonly Dictionary<int, int> _lastAppliedByInstanceId = new Dictionary<int, int>();

        private void Update()
        {
            if (Time.realtimeSinceStartup < _nextScanTime)
            {
                return;
            }

            var interval = Plugin.ScanInterval?.Value ?? 0.5f;
            _nextScanTime = Time.realtimeSinceStartup + Mathf.Max(0.1f, interval);

            TryApplyTraderGridLayout();
        }

        private void TryApplyTraderGridLayout()
        {
            var desiredColumns = Mathf.Clamp(Plugin.TraderColumns?.Value ?? 8, 1, 20);
            var groups = Resources.FindObjectsOfTypeAll<GridLayoutGroup>();

            foreach (var grid in groups)
            {
                if (grid == null || grid.gameObject == null)
                {
                    continue;
                }

                if (!grid.gameObject.activeInHierarchy)
                {
                    continue;
                }

                if (!LooksLikeTraderGrid(grid))
                {
                    continue;
                }

                ApplyColumns(grid, desiredColumns);
            }
        }

        private void ApplyColumns(GridLayoutGroup grid, int desiredColumns)
        {
            var id = grid.GetInstanceID();

            if (_lastAppliedByInstanceId.TryGetValue(id, out var last) &&
                last == desiredColumns &&
                grid.constraint == GridLayoutGroup.Constraint.FixedColumnCount &&
                grid.constraintCount == desiredColumns)
            {
                return;
            }

            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = desiredColumns;
            LayoutRebuilder.MarkLayoutForRebuild(grid.transform as RectTransform);

            _lastAppliedByInstanceId[id] = desiredColumns;

            if (Plugin.DebugLogging?.Value == true)
            {
                Debug.Log($"[{Plugin.PluginGuid}] Applied trader columns = {desiredColumns} to {GetGameObjectPath(grid.gameObject)}");
            }
        }

        private static bool LooksLikeTraderGrid(GridLayoutGroup grid)
        {
            var transform = grid.transform;
            var childCount = transform.childCount;

            // Trader grids contain a moderate number of trader cards.
            // This avoids touching stash/item/flea grids, which usually contain many item cells.
            if (childCount < 4 || childCount > 40)
            {
                return false;
            }

            var path = GetGameObjectPath(grid.gameObject).ToLowerInvariant();
            if (path.Contains("trader"))
            {
                return ContainsTraderText(transform);
            }

            return ContainsTraderText(transform);
        }

        private static bool ContainsTraderText(Transform root)
        {
            var found = 0;
            var texts = root.GetComponentsInChildren<Component>(true);

            foreach (var component in texts)
            {
                if (component == null)
                {
                    continue;
                }

                var text = TryReadText(component);
                if (string.IsNullOrWhiteSpace(text))
                {
                    continue;
                }

                var lower = text.ToLowerInvariant();
                for (var i = 0; i < KnownTraderNames.Length; i++)
                {
                    if (lower.Contains(KnownTraderNames[i]))
                    {
                        found++;
                        if (found >= 2)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static string TryReadText(Component component)
        {
            try
            {
                // UnityEngine.UI.Text
                if (component is Text uiText)
                {
                    return uiText.text;
                }

                // TextMeshProUGUI / TMP_Text without adding a hard compile-time dependency.
                var type = component.GetType();
                if (!type.FullName.StartsWith("TMPro.", StringComparison.Ordinal))
                {
                    return null;
                }

                var property = type.GetProperty("text", BindingFlags.Instance | BindingFlags.Public);
                return property?.GetValue(component, null) as string;
            }
            catch
            {
                return null;
            }
        }

        private static string GetGameObjectPath(GameObject go)
        {
            if (go == null)
            {
                return string.Empty;
            }

            var path = go.name;
            var parent = go.transform.parent;

            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }
    }
}
