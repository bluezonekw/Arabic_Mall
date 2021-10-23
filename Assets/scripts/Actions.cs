using UnityEngine;
using System.Collections;

public class Actions : MonoBehaviour {

	public Animator[] animator;
	const int countOfDamageAnimations = 3;
	int lastDamageAnimation = -1;

	void Awake () {
        if (!PlayerPrefs.HasKey("MallGender"))
        {
            PlayerPrefs.SetInt("MallGender", 0);
        }
        animator[PlayerPrefs.GetInt("MallGender")].gameObject.SetActive(true);

        ///animator = GetComponent<Animator> ();
    }
    public void changeGender()
    {
        for (int i = 0; i < animator.Length; i++)
        {
            animator[i].gameObject.SetActive(false);
        }
        animator[PlayerPrefs.GetInt("MallGender")].gameObject.SetActive(true);
    }
	public void Stay () {

        animator[PlayerPrefs.GetInt("MallGender")].SetBool("Aiming", false);
            animator[PlayerPrefs.GetInt("MallGender")].SetFloat("Speed", 0f);
        Debug.Log(animator[0].avatar.name);
    }

    public void Walk (float value) {


        animator[PlayerPrefs.GetInt("MallGender")].SetFloat ("Forward", value);

        if (value >= .7f)
        {
            this.gameObject.GetComponent<OwnThirdPersonController>().walkSpeed = 5f;
        }
        else 
        {
            this.gameObject.GetComponent<OwnThirdPersonController>().walkSpeed = 4.5f;

        }
       
        //animator.speed = value*3;
    }

	public void Run () {
		animator[PlayerPrefs.GetInt("MallGender")].SetBool("Aiming", false);
		animator[PlayerPrefs.GetInt("MallGender")].SetFloat ("Speed", 1f);
	}

    public void Attack()
    {
        Aiming();
        animator[PlayerPrefs.GetInt("MallGender")].SetTrigger("Attack");
    }

    public void Death () {
		if (animator[PlayerPrefs.GetInt("MallGender")].GetCurrentAnimatorStateInfo (0).IsName ("Death"))
			animator[PlayerPrefs.GetInt("MallGender")].Play("Idle", 0);
		else
			animator[PlayerPrefs.GetInt("MallGender")].SetTrigger ("Death");
	}

	public void Damage () {
		if (animator[PlayerPrefs.GetInt("MallGender")].GetCurrentAnimatorStateInfo (0).IsName ("Death")) return;
		int id = Random.Range(0, countOfDamageAnimations);
		if (countOfDamageAnimations > 1)
			while (id == lastDamageAnimation)
				id = Random.Range(0, countOfDamageAnimations);
		lastDamageAnimation = id;
		animator[PlayerPrefs.GetInt("MallGender")].SetInteger ("DamageID", id);
		animator[PlayerPrefs.GetInt("MallGender")].SetTrigger ("Damage");
    }

    public void Jump()
    {
        animator[PlayerPrefs.GetInt("MallGender")].SetBool("Squat", false);
        animator[PlayerPrefs.GetInt("MallGender")].SetFloat("Speed", 0f);
        animator[PlayerPrefs.GetInt("MallGender")].SetBool("Aiming", false);
        animator[PlayerPrefs.GetInt("MallGender")].SetTrigger("Jump");
    }

    public void Aiming () {
		animator[PlayerPrefs.GetInt("MallGender")].SetBool ("Squat", false);
		animator[PlayerPrefs.GetInt("MallGender")].SetFloat ("Speed", 0f);
		animator[PlayerPrefs.GetInt("MallGender")].SetBool("Aiming", true);
	}

	public void Sitting (bool sitting) {
		animator[PlayerPrefs.GetInt("MallGender")].SetBool ("Squat", sitting);
		animator[PlayerPrefs.GetInt("MallGender")].SetBool("Aiming", false);
	}
}
