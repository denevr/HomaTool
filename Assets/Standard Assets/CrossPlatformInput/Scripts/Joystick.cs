using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}

		public int MovementRange = 100;
		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public float sens = 50;
		
		Vector3 m_StartPos;
		bool m_UseX; // Toggle for using the x axis
		bool m_UseY; // Toggle for using the Y axis

		public Transform joystickPivot;
		bool moved = false;

		void LateUpdate()
		{
			CrossPlatformInputManager.SetAxis("Horizontal", 0);
		}

        void Start()
        {
            m_StartPos = joystickPivot.position;
			m_UseX = axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal;
			m_UseY = axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical;

		}

		void UpdateVirtualAxes(Vector3 value)
		{
			var delta = m_StartPos - value;
			delta.y = -delta.y;
			delta /= MovementRange;
			if (m_UseX)
			{
				CrossPlatformInputManager.SetAxis("Horizontal", -delta.x);
			}

			if (m_UseY)
			{
				CrossPlatformInputManager.SetAxis("Vertical", delta.y);
			}
		}

		public void OnDrag(PointerEventData data)
		{
			if (moved) return;

			Vector3 deltaPos = Vector3.zero;

			if (m_UseX)
			{
				int delta = (int)(data.position.x - m_StartPos.x);
				delta = Mathf.Clamp(delta, - MovementRange, MovementRange);
				deltaPos.x = delta;
			}

			if (m_UseY)
			{
				int delta = (int)(data.position.y - m_StartPos.y);
				delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
				deltaPos.y = delta;
			}
			if (deltaPos.magnitude >= sens)
			{
				moved = true;
				joystickPivot.position = new Vector3(m_StartPos.x + deltaPos.x, m_StartPos.y + deltaPos.y, m_StartPos.z + deltaPos.z);
				UpdateVirtualAxes(joystickPivot.position);
			}
		}


		public void OnPointerUp(PointerEventData data)
		{
		}

		public void OnPointerDown(PointerEventData data) 
		{
			joystickPivot.position = data.position;
			m_StartPos = data.position;
			moved = false;
		}
	}
}